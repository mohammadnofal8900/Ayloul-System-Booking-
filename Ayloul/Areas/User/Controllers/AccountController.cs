using Ayloul.Areas.User.Models;
using Ayloul.Data;
using Ayloul.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using static Ayloul.Areas.User.Models.LoginVm;

namespace Ayloul.Areas.User.Controllers
{
    [Area("User")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;
        private bool lockoutOnFailure;
        private readonly IHttpClientFactory _httpClientFactory;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ApplicationDbContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public IActionResult Regester()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Regester(RegesterVm model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            try
            {
                // إنشاء HttpClient للـ API
                var client = _httpClientFactory.CreateClient("AyloulApi");
                // إرسال بيانات التسجيل إلى API
                var response = await client.PostAsJsonAsync( "api/Auth/Register",
                    new { 
                        model.Full_Name,
                        model.Email,
                        model.Phone_Number,
                        model.Password,
                        model.ConfirmPassword });

                // إذا فشل التسجيل
                if (!response.IsSuccessStatusCode)
                {
                    var errorContent =
                        await response.Content.ReadAsStringAsync();

                    ModelState.AddModelError(
                        string.Empty,
                        "Email or Password is incorrect.");

                    return View(model);
                }

                // قراءة Response من API
                var responseContent = await response.Content.ReadAsStringAsync();


                var registerResponse = JsonSerializer.Deserialize<RegesterResponseVm>( responseContent, new JsonSerializerOptions { 
                    PropertyNameCaseInsensitive = true });
                // التأكد من وجود Token
                if (registerResponse == null || string.IsNullOrEmpty(registerResponse.Token))
                {
                    ModelState.AddModelError( string.Empty, "Registration failed. Token was not returned.");
                    return View(model);
                }
                // قراءة JWT
                var handler = new JwtSecurityTokenHandler();

                var jwtToken = handler.ReadJwtToken(registerResponse.Token);

                // استخراج Claims من JWT
                var claims = jwtToken.Claims.ToList();

                // إضافة الـ Token نفسه كـ Claim
                claims.Add( new Claim( "AccessToken", registerResponse.Token)); 

                // إنشاء Identity
                var identity = new ClaimsIdentity( claims, IdentityConstants.ApplicationScheme);

                // إنشاء Principal
                var principal = new ClaimsPrincipal(identity);

                // تسجيل الدخول باستخدام Cookie
                await HttpContext.SignInAsync( IdentityConstants.ApplicationScheme, principal, new AuthenticationProperties 
                { IsPersistent = model.RememberMe, ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8) });

                // الصفحة الافتراضية بعد التسجيل
                return RedirectToAction( "Index", "Home", new { area = "" });
            
            
            
            
            
            }
            catch (Exception) {
                ModelState.AddModelError(string.Empty, "An error occurred while trying to register.");
                return View(model);
            }



            //        var user = new ApplicationUser
            //        {
            //            UserName=model.Full_Name,
            //            FUllName=model.Full_Name,
            //            Email = model.Email,
            //            PhoneNumber=model.Phone_Number,
            //            Is_Active=true
            //        };
            //        var result= await _userManager.CreateAsync(user,model.Password);
            //        user.EmailConfirmed = true;
            //        if (result.Succeeded)
            //        {
            //            var customer = new Customer
            //            {
            //                UserId = user.Id,
            //                Is_Active = true,
            //            };

            //            await _context.AddAsync(customer);
            //            await _context.SaveChangesAsync();

            //            var claims = new List<Claim>
            //{
            //    new Claim("FullName", user.FUllName)
            //};

            //            await _signInManager.SignInWithClaimsAsync(
            //                user,
            //                model.RememberMe,
            //                claims
            //            );

            //            return RedirectToAction("Index","home",new {area =""});
            //        }
            //        foreach(var item in result.Errors)
            //        {
            //            ModelState.AddModelError("", item.Description);
            //        }
            //        return View(model);

        }



        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVm model, string? return_url = null)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ReturnUrl = return_url;
                return View(model);
            }
            try
            {
                // إنشاء HttpClient للـ API
                var client = _httpClientFactory.CreateClient("AyloulApi");

                // إرسال Email + Password إلى API
                var response = await client.PostAsJsonAsync(
                    "api/Auth/Login",
                    new
                    {
                        model.Email,
                        model.Password,
                        model.RememberMe
                    });

                // إذا تسجيل الدخول فشل
                if (!response.IsSuccessStatusCode)
                {
                    ModelState.AddModelError(
                        string.Empty,
                        "Email or Password is incorrect.");

                    ViewBag.ReturnUrl = return_url;

                    return View(model);
                }

                // قراءة Response من API
                var responseContent = await response.Content.ReadAsStringAsync();

                var loginResponse =
                    JsonSerializer.Deserialize<LoginResponseVm>(
                        responseContent,
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });

                // التأكد من وجود Token
                if (loginResponse == null ||
                    string.IsNullOrEmpty(loginResponse.Token))
                {
                    ModelState.AddModelError(
                        string.Empty,
                        "Login failed. Token was not returned.");

                    ViewBag.ReturnUrl = return_url;

                    return View(model);
                }

                // قراءة الـ JWT
                var handler = new JwtSecurityTokenHandler();

                var jwtToken = handler.ReadJwtToken(loginResponse.Token);

                // استخراج Claims من الـ JWT
                var claims = jwtToken.Claims.ToList();

                // إضافة Token نفسه كـ Claim
                claims.Add(
                    new Claim(
                        "AccessToken",
                        loginResponse.Token));

                // إنشاء Identity
                var identity = new ClaimsIdentity(
                    claims,
                    IdentityConstants.ApplicationScheme);

                var principal = new ClaimsPrincipal(identity);

                // تسجيل الدخول باستخدام Cookie
                await HttpContext.SignInAsync(
                    IdentityConstants.ApplicationScheme,
                    principal,
                    new AuthenticationProperties
                    {
                        IsPersistent = model.RememberMe,
                        ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8)
                    });

                // الرجوع للصفحة المطلوبة
                if (!string.IsNullOrEmpty(return_url) &&
                    Url.IsLocalUrl(return_url))
                {
                    return Redirect(return_url);
                }

                return RedirectToAction("Index", "Home", new { area = "" });
            }
            catch (Exception)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "An error occurred while trying to login.");

                ViewBag.ReturnUrl = return_url;

                return View(model);
            }

            //var user = await _userManager.FindByEmailAsync(model.Email);

            //if (user == null)
            //{
            //    ModelState.AddModelError("", "Email does not exist.");
            //    return View(model);
            //}

            //if (!user.Is_Active)
            //{
            //    ModelState.AddModelError("", "Your account is not active.");
            //    return View(model);
            //}

            //var passwordValid = await _userManager.CheckPasswordAsync(
            //    user,
            //    model.Password
            //);

            //if (!passwordValid)
            //{
            //    ModelState.AddModelError("", "Invalid password.");
            //    return View(model);
            //}


            //var claims = new List<Claim>
            //{
            //    new Claim("FullName", user.FUllName)
            //};


            //await _signInManager.SignInWithClaimsAsync(
            //    user,
            //    model.RememberMe,
            //    claims
            //);


            //if (!string.IsNullOrEmpty(return_url)
            //    && Url.IsLocalUrl(return_url))
            //{
            //    return Redirect(return_url);
            //}


            //return RedirectToAction("Index", "Home", new { area = "" });
        }




        [HttpGet]
        [AllowAnonymous]
        public IActionResult GoogleLogin()
        {
            var apiBaseUrl = "https://localhost:7067";

            var callbackUrl = Url.Action(
                "GoogleCallback",
                "Account",
                new { area = "User" },
                Request.Scheme);

            var url =
                $"{apiBaseUrl}/api/Auth/GoogleLogin" +
                $"?returnUrl={Uri.EscapeDataString(callbackUrl!)}";

            return Redirect(url);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GoogleCallback(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                TempData["Error"] = "Google login failed.";
                return RedirectToAction("Login", "Account", new { area = "User" });
            }

            try
            {
                var handler = new JwtSecurityTokenHandler();

                var jwtToken = handler.ReadJwtToken(token);

                var claims = jwtToken.Claims.ToList();

                claims.Add(
                    new Claim(
                        "AccessToken",
                        token));

                var identity = new ClaimsIdentity(
                    claims,
                    IdentityConstants.ApplicationScheme);

                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(
                    IdentityConstants.ApplicationScheme,
                    principal,
                    new AuthenticationProperties
                    {
                        IsPersistent = true,
                        ExpiresUtc = DateTimeOffset.UtcNow.AddHours(2)
                    });

                return RedirectToAction(
                    "Index",
                    "Home",
                    new { area = "" });
            }
            catch
            {
                TempData["Error"] = "Google login failed.";
                return RedirectToAction(
                    "Login",
                    "Account",
                    new { area = "User" });
            }
        }


        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "home", new { area = "" });
        }



    }
}
            

