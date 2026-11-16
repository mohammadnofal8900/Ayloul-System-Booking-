using Ayloul.Areas.Admin.Models;
using Ayloul.Areas.Admin.Services;
using Ayloul.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Ayloul.Areas.Admin.Services;

namespace Ayloul.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SearvicesApiController : Controller
    {
        private readonly ApiResponseHandler _apiResponseHandler;
        private readonly IHttpClientFactory _httpClientFactory;

        public SearvicesApiController(IHttpClientFactory httpClientFactory, ApiResponseHandler apiResponseHandler)
        {
            
            _apiResponseHandler = apiResponseHandler;
            _httpClientFactory = httpClientFactory;
        }

        private HttpClient GetApiClint()
        {
            var clint = _httpClientFactory.CreateClient("AyloulApi");
            var token = User.FindFirst("AccessToken")?.Value;
            if (!string.IsNullOrEmpty(token))
            {
                clint.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer",token);
            }
            return clint;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var client = GetApiClint();

            var response = await client.GetAsync("api/Services");

            var errorResult = _apiResponseHandler.Handle(response,
                this
                );

            if (errorResult != null)
                return errorResult;

            var json = await response.Content.ReadAsStringAsync();

            Console.WriteLine(json);

            var services =
                await response.Content.ReadFromJsonAsync<List<ServicesApiVm>>();

            return View(services);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ServicesApiVm model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                string? fileName = null;

                // حفظ الصورة في MVC
                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    fileName = Guid.NewGuid().ToString()
                        + Path.GetExtension(model.ImageFile.FileName);

                    var folderPath = Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "wwwroot",
                        "Images",
                        "Service"
                    );

                    if (!Directory.Exists(folderPath))
                        Directory.CreateDirectory(folderPath);

                    var filePath = Path.Combine(folderPath, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.ImageFile.CopyToAsync(stream);
                    }
                }

                var client = GetApiClint();

                using var content = new MultipartFormDataContent();

                content.Add(
                    new StringContent(model.Service_Name),
                    "Service_Name");

                content.Add(
                    new StringContent(model.Price.ToString()),
                    "Price");

                content.Add(
                    new StringContent(model.Is_Active.ToString()),
                    "Is_Active");

                // نرسل اسم الصورة فقط للـ API
                if (!string.IsNullOrEmpty(fileName))
                {
                    content.Add(
                        new StringContent(fileName),
                        "Img");
                }

                var response = await client.PostAsync(
                    "api/Services",
                    content);
                var errorResult = _apiResponseHandler.Handle(response, this);

                if (errorResult != null)
                    return errorResult;

                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                ModelState.AddModelError(
                    "",
                    "An error occurred while creating the service.");

                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var client = GetApiClint();

            var response = await client.GetAsync($"api/Services/{id}");

            var errorResult = _apiResponseHandler.Handle(response, this);

            if (errorResult != null)
                return errorResult;

            var services = await response.Content
                .ReadFromJsonAsync<List<ServicesApiVm>>();

            if (services == null || !services.Any())
                return NotFound();

            var service = services.First();

            return View(service);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ServicesApiVm model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var client = GetApiClint();

                string? fileName = null;

                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    fileName = Guid.NewGuid().ToString()
                        + Path.GetExtension(model.ImageFile.FileName);

                    var folderPath = Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "wwwroot",
                        "Images",
                        "Service"
                    );

                    if (!Directory.Exists(folderPath))
                        Directory.CreateDirectory(folderPath);

                    var filePath = Path.Combine(folderPath, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.ImageFile.CopyToAsync(stream);
                    }
                }

                using var content = new MultipartFormDataContent();

                content.Add(
                    new StringContent(model.Service_Name),
                    "Service_Name"
                );

                content.Add(
                    new StringContent(model.Price.ToString()),
                    "Price"
                );

                content.Add(
                    new StringContent(model.Is_Active.ToString()),
                    "Is_Active"
                );

                if (!string.IsNullOrEmpty(fileName))
                {
                    content.Add(
                        new StringContent(fileName),
                        "Img"
                    );
                }

                var response = await client.PutAsync(
                    $"api/Services/{model.Service_Id}",
                    content
                );

                var errorResult = _apiResponseHandler.Handle(
                    response,
                    this
                );

                if (errorResult != null)
                    return errorResult;

                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                ModelState.AddModelError(
                    "",
                    "An error occurred while updating the service."
                );

                return View(model);
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var client = GetApiClint();

                var response = await client.DeleteAsync($"api/Services/{id}");

                var errorResult = _apiResponseHandler.Handle(response, this);

                if (errorResult != null)
                    return errorResult;

                TempData["SuccessMessage"] = "Service deleted successfully.";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "An error occurred while deleting the service.";

                return RedirectToAction(nameof(Index));
            }
        }
    }
}
