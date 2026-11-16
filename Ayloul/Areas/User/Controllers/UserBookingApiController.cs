using Ayloul.Areas.Admin.Models;
using Ayloul.Areas.Admin.Services;
using Ayloul.Areas.User.Models;
using Ayloul.Data;
using Ayloul.Models;
using DocumentFormat.OpenXml.ExtendedProperties;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.Json;
using System.Threading.Tasks;

namespace Ayloul.Areas.User.Controllers
{

    [Area("User")]
    [Authorize]
    public class UserBookingApiController : Controller
    {


        private readonly ApiResponseHandler _apiResponseHandler;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly UserManager<ApplicationUser> _userManager;


        public UserBookingApiController(IHttpClientFactory httpClientFactory, ApiResponseHandler apiResponseHandler,UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _apiResponseHandler = apiResponseHandler;
            _httpClientFactory = httpClientFactory;
        }

        private HttpClient GetApiClint()
        {
            var clint = _httpClientFactory.CreateClient("AyloulApi");
            var token = User.FindFirst("AccessToken")?.Value;
            if (!string.IsNullOrEmpty(token))
            {
                clint.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
            return clint;
        }




        [HttpGet]
        public async Task<IActionResult> AddBooking()
        {
            var client = GetApiClint();

            await LoadDropDownList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddBooking(UserBookingEditVm model)
        {
            if (!ModelState.IsValid)
            {
                await LoadDropDownList();
                return View(model);
            }


            var client = GetApiClint();

            var userId = _userManager.GetUserId(User);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            client.DefaultRequestHeaders.Remove("X-User-Id");
            client.DefaultRequestHeaders.Add("X-User-Id", userId);


            string? newFileName = model.Note_Image;

            // إذا اختار المستخدم صورة جديدة
            if (model.ImageFile != null && model.ImageFile.Length > 0)
            {
                newFileName = Guid.NewGuid().ToString()
                    + Path.GetExtension(model.ImageFile.FileName);

                var folderPath = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot",
                    "Images",
                    "Booking"
                );

                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                var filePath = Path.Combine(
                    folderPath,
                    newFileName
                );

                using (var stream = new FileStream(
                    filePath,
                    FileMode.Create))
                {
                    await model.ImageFile.CopyToAsync(stream);
                }

            }
            using var content = new MultipartFormDataContent();


                content.Add(
            new StringContent(model.BookingId.ToString()),
            "BookingId"
        );
                content.Add(
            new StringContent(model.Name.ToString()),
            "Name"
        );
                content.Add(
           new StringContent(model.Order_Date.ToString()),
           "OrderDate"
       );
            content.Add(
           new StringContent(model.Order_Date.ToString("yyyy-MM-dd")),
           "DeliverDate"
       );
            content.Add(
            new StringContent(model.Description.ToString()),
            "Description"
        );
            if (model.Note != null)
            {
                content.Add(
                    new StringContent(model.Note),
                    "Note"
                );
            }

            content.Add(
           new StringContent(model.Event_Date.ToString()),
           "EventDate"
       );
                content.Add(
           new StringContent(model.Location_Id.ToString()),
           "LocationId"
       );
                
                content.Add(
           new StringContent(model.Background_Id.ToString()),
           "BackgroundId"
       );
                content.Add(
           new StringContent(model.FrameColor_Id.ToString()),
           "FrameColorId"
       );
                content.Add(
           new StringContent(model.RoseColor1_Id.ToString()),
           "RoseColor1Id"

       );
            if (model.RoseColor2_Id != null )
            {
               
                    content.Add(
           new StringContent(model.RoseColor2_Id.ToString()),
           "RoseColor2Id"

       );
                
            }
            content.Add(
    new StringContent(model.Phone_Number ?? ""),
    "PhoneNumber"
);

            content.Add(
           new StringContent(model.RoseType_Id.ToString()),
           "RoseTypeId"

       );
                if (model.Service_Id != null && model.Service_Id.Count > 0)
                {
                    foreach (var serviceId in model.Service_Id.Distinct())
                    {
                        content.Add(
                            new StringContent(serviceId.ToString()),
                            "ServiceId"
                        );
                    }
                }
                if (!string.IsNullOrEmpty(newFileName))
                {
                    content.Add(
                        new StringContent(newFileName),
                        "NoteImage"
                    );
                }



            try
            {
                var response = await client.PostAsync(
"api/UserBooking/AddBooking",
content
);

                var responseBody =
                    await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Error404", "Home", new { area = "" });
                    
                }

                var confirmation =
    JsonSerializer.Deserialize<BookingConfirmViewModel>(
        responseBody,
        new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

                if (confirmation == null)
                {
                    ModelState.AddModelError("", "Could not prepare booking confirmation.");

                    await LoadDropDownList();

                    return View(model);
                }

                TempData["BookingConfirmation"] =
                    JsonSerializer.Serialize(confirmation);

                return View("Confirmation", confirmation);
            }
            catch (HttpRequestException ex)
            {
                return Content(
                    $"HTTP ERROR:\n{ex.Message}\n\n" +
                    $"Inner:\n{ex.InnerException?.Message}"
                );
            }


        }



        [HttpGet]
        public IActionResult Confirmation()
        {
            var json = TempData["BookingConfirmation"] as string;

            if (string.IsNullOrEmpty(json))
            {
                return RedirectToAction("AddBooking");
            }

            var confirmation =
                JsonSerializer.Deserialize<BookingConfirmViewModel>(
                    json,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

            if (confirmation == null)
            {
                return RedirectToAction("AddBooking");
            }

            return View(confirmation);
        }


        [HttpPost]
        public async Task<IActionResult> ConfirmCash()
        {
            var json = TempData["BookingConfirmation"] as string;

            if (string.IsNullOrEmpty(json))
            {
                return RedirectToAction("AddBooking");
            }

            var confirmation =
                JsonSerializer.Deserialize<BookingConfirmViewModel>(
                    json,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

            if (confirmation == null)
            {
                return RedirectToAction("AddBooking");
            }

            var client = GetApiClint();

            var userId = _userManager.GetUserId(User);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            client.DefaultRequestHeaders.Remove("X-User-Id");
            client.DefaultRequestHeaders.Add("X-User-Id", userId);

            var response = await client.PostAsJsonAsync(
                "api/UserBooking/ConfirmCash",
                confirmation
            );

            var responseBody =
                await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                return Content(responseBody);
            }
            TempData["BookingSuccess"] =
       "Your booking has been confirmed successfully! 🎉";

            return RedirectToAction("Index", "Home", new { area = "" });
        }





        [HttpPost]
        public IActionResult ConfirmVisa()
        {
            var json = TempData["BookingConfirmation"] as string;

            if (string.IsNullOrEmpty(json))
            {
                return RedirectToAction("AddBooking");
            }

            var confirmation =
                JsonSerializer.Deserialize<BookingConfirmViewModel>(
                    json,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

            if (confirmation == null)
            {
                return RedirectToAction("AddBooking");
            }

            TempData["BookingConfirmation"] =
                JsonSerializer.Serialize(confirmation);

            return View("Visa", new VisaPaymentViewModel());
        }



        [HttpPost]
        public async Task<IActionResult> PayVisa(
    VisaPaymentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Visa", model);
            }

            var json = TempData["BookingConfirmation"] as string;

            if (string.IsNullOrEmpty(json))
            {
                return RedirectToAction("AddBooking");
            }

            var confirmation =
                JsonSerializer.Deserialize<BookingConfirmViewModel>(
                    json,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

            if (confirmation == null)
            {
                return RedirectToAction("AddBooking");
            }

            var client = GetApiClint();

            var userId = _userManager.GetUserId(User);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            client.DefaultRequestHeaders.Remove("X-User-Id");
            client.DefaultRequestHeaders.Add("X-User-Id", userId);

            var request = new
            {
                Booking = confirmation,

                CardHolderName = model.CardHolderName,
                CardNumber = model.CardNumber,
                ExpiryDate = model.ExpiryDate,
                CVV = model.CVV
            };

            var response = await client.PostAsJsonAsync(
                "api/UserBooking/ConfirmVisa",
                request
            );

            var responseBody =
                await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                return Content(responseBody);
            }

            TempData["BookingSuccess"] =
                "Your booking and Visa payment have been completed successfully! 🎉";

            return RedirectToAction("Index", "Home", new { area = "" });
        }


        [HttpGet]
        public async Task<IActionResult> MyBookings()
        {
            // ==============================
            // Get API Client
            // ==============================

            var client = GetApiClint();


            // ==============================
            // Get current Identity User
            // ==============================

            var userId = _userManager.GetUserId(User);

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();


            // ==============================
            // Send User ID to API
            // ==============================

            client.DefaultRequestHeaders.Remove("X-User-Id");

            client.DefaultRequestHeaders.Add(
                "X-User-Id",
                userId
            );


            // ==============================
            // Call API
            // ==============================

            var response = await client.GetAsync(
                "api/UserBooking/MyBookings"
            );


            // ==============================
            // Check response
            // ==============================

            var responseBody =
                await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                return RedirectToAction("Error404", "Home", new { area = "" });
            }


            // ==============================
            // Deserialize
            // ==============================

            var bookings =
                JsonSerializer.Deserialize<List<MyBookingDto>>(
                    responseBody,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }
                );


            // ==============================
            // Return View
            // ==============================

            return View(bookings ?? new List<MyBookingDto>());
        }



        public async Task LoadDropDownList()
        {
            var client = GetApiClint();


            var locations =
                await client.GetFromJsonAsync<List<LocationDropdownDto>>(
                    "api/UserBooking/GetLocations");


            var RoseColor =
                await client.GetFromJsonAsync<List<RoseColorDropdownDto>>(
                    "api/UserBooking/GetRoseColor");

            var Bacground =
                await client.GetFromJsonAsync<List<BackgroundDropdownDto>>(
                    "api/UserBooking/GetBachground");


            
            var RoseType =
                await client.GetFromJsonAsync<List<RoseTypeDropdownDto>>(
                    "api/UserBooking/GetRoseType");

            var FrameColor =
                await client.GetFromJsonAsync<List<FrameColorDropdownDto>>(
                    "api/UserBooking/GetFrameColor");

            var services =
                await client.GetFromJsonAsync<List<ServiceDropdownDto>>(
                    "api/UserBooking/GetServices");


            // تأكد أن القوائم ليست null
            Bacground ??= new List<BackgroundDropdownDto>();
            RoseColor ??= new List<RoseColorDropdownDto>();
            locations ??= new List<LocationDropdownDto>();
            RoseType ??= new List<RoseTypeDropdownDto>();
            FrameColor ??= new List<FrameColorDropdownDto>();
            services ??= new List<ServiceDropdownDto>();


            ViewBag.Background = Bacground
    .Select(x => new SelectListItem
    {
        Value = x.BackgroundId.ToString(),
        Text = x.BackgroundColor ?? ""
    })
    .ToList();

            ViewBag.RoseColor = new SelectList(
                RoseColor,
                "RoseColor_Id",
                "RoseColor_Name");

            ViewBag.Location = new SelectList(
                locations,
                "LocationId",
                "City");

            ViewBag.RoseType = new SelectList(
                RoseType,
                "RoseType_Id",
                "RoseType_Name");

            ViewBag.FrameColor = new SelectList(
                FrameColor,
                "FrameColor_Id",
                "FrameColor_Color");

            ViewBag.Service = new SelectList(
                services,
                "Service_Id",
                "Service_Name");

            ViewBag.BackgroundList = Bacground;
            ViewBag.RoseColorList = RoseColor;
            ViewBag.LocationList = locations;
            ViewBag.RoseTypeList = RoseType;
            ViewBag.FrameColorList = FrameColor;
            ViewBag.ServiceList = services;
        }
    }
}
