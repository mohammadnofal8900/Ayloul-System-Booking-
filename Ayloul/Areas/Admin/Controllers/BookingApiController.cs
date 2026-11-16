using Ayloul.Areas.Admin.Models;
using Ayloul.Areas.Admin.Services;
using Ayloul.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static Ayloul.Models.Booking;

namespace Ayloul.Areas.Admin.Controllers
{

    [Authorize(Roles = "Super Admin,Admin")]
    [Area("Admin")]
    [Authorize]
    public class BookingApiController : Controller
    {
        
        private readonly ApiResponseHandler _apiResponseHandler;
        private readonly IHttpClientFactory _httpClientFactory;


        public BookingApiController(IHttpClientFactory httpClientFactory, ApiResponseHandler apiResponseHandler)
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
                clint.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
            return clint;
        }


        [HttpGet]
        public async Task<IActionResult> GetBookingByStatus(BookingStatus status)
        {
            var client = GetApiClint();

            var response = await client.GetAsync($"api/Booking/status/{status}");


            var errorResult = _apiResponseHandler.Handle(response,
                this
                );

            if (errorResult != null)
                return errorResult;

            var json = await response.Content.ReadAsStringAsync();

            Console.WriteLine(json);

            var booking =
                await response.Content.ReadFromJsonAsync<List<BookingVm>>();

            return View(booking);
        }

        [HttpGet]
        public async Task<IActionResult> ViewInfoBooking(int id)
        {
            var client = GetApiClint();

            var response = await client.GetAsync($"api/Booking/{id}");


            var errorResult = _apiResponseHandler.Handle(response,
                this
                );

            if (errorResult != null)
                return errorResult;

            var json = await response.Content.ReadAsStringAsync();

            Console.WriteLine(json);

            var bookingInfo =
                await response.Content.ReadFromJsonAsync<BookingInfoVm>();


            await LoadDropDownList();

            return View(bookingInfo);
        }

        [HttpGet]
        
        public async Task<IActionResult> EditBooking(int id)
        {
            var client = GetApiClint();

            var response = await client.GetAsync($"api/Booking/{id}");

            var errorResult = _apiResponseHandler.Handle(response, this);

            if (errorResult != null)
                return errorResult;

            var booking =
                await response.Content.ReadFromJsonAsync<BookingEditVm>();

            await LoadDropDownList();

            return View(booking);
        }


        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditBooking(BookingEditVm model, int id)
        {
            if (!ModelState.IsValid)
            {
                await LoadDropDownList();
                return View(model);
            }

            try
            {
                var client = GetApiClint();

                // اسم الصورة الحالية
                string? newFileName = model.NoteImage;

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

                    // حذف الصورة القديمة
                    if (!string.IsNullOrEmpty(model.NoteImage))
                    {
                        var oldFilePath = Path.Combine(
                            folderPath,
                            model.NoteImage
                        );

                        if (System.IO.File.Exists(oldFilePath))
                        {
                            System.IO.File.Delete(oldFilePath);
                        }
                    }
                }
                using var content = new MultipartFormDataContent();

              


                // BookingId
                content.Add(
                    new StringContent(model.BookingId.ToString()),
                    "BookingId"
                );

                // البيانات الأساسية
                content.Add(
                    new StringContent(model.Name ?? ""),
                    "Name"
                );

                content.Add(
                    new StringContent(model.EventDate ?? ""),
                    "EventDate"
                );

                content.Add(
                    new StringContent(model.Description ?? ""),
                    "Description"
                );

                content.Add(
                    new StringContent(model.Note ?? ""),
                    "Note"
                );

                // العلاقات
                if (model.ServiceId != null && model.ServiceId.Count > 0)
                {
                    foreach (var serviceId in model.ServiceId.Distinct())
                    {
                        content.Add(
                            new StringContent(serviceId.ToString()),
                            "ServiceId"
                        );
                    }
                }

                content.Add(
                    new StringContent(model.CustomerId.ToString() ?? ""),
                    "CustomerId"
                );

                content.Add(
                    new StringContent(model.LocationId.ToString() ?? ""),
                    "LocationId"
                );

                content.Add(
                    new StringContent(model.FrameColor.ToString() ?? ""),
                    "FrameColor"
                );

                content.Add(
                    new StringContent(model.Background?.ToString() ?? ""),
                    "Background"
                );

                content.Add(
                    new StringContent(model.RoseType.ToString() ?? ""),
                    "RoseType"
                );

                content.Add(
                    new StringContent(model.RoseColor1_Id.ToString() ?? ""),
                    "RoseColor1_Id"
                );

                content.Add(
                    new StringContent(model.RoseColor2_Id?.ToString() ?? ""),
                    "RoseColor2_Id"
                );

                // Status
                content.Add(
                    new StringContent(model.bookingStatus.ToString()),
                    "bookingStatus"
                );

                // Active
                content.Add(
                    new StringContent(model.IsActive.ToString()),
                    "IsActive"
                );

                // الصورة
                if (!string.IsNullOrEmpty(newFileName))
                {
                    content.Add(
                        new StringContent(newFileName),
                        "NoteImage"
                    );
                }

                // إرسال الطلب للـ API
                var response = await client.PutAsync(
                    $"api/Booking/{id}",
                    content
                );


                var errorResult = _apiResponseHandler.Handle(
                    response,
                    this
                );

                if (errorResult != null)
                    return errorResult;

                return RedirectToAction("Booking", "Booking", new { area = "Admin" });
            }
            catch (Exception)
            {
                ModelState.AddModelError(
                    "",
                    "An error occurred while updating the booking."
                );

                return View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            var client = GetApiClint();

            var response = await client.DeleteAsync($"api/Booking/{id}");

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Booking deleted successfully.";
                return RedirectToAction("Booking", "Booking", new { area = "Admin" });
            }

            var errorMessage = "Failed to delete booking.";

            try
            {
                var errorContent = await response.Content.ReadAsStringAsync();

                if (!string.IsNullOrEmpty(errorContent))
                {
                    errorMessage = errorContent;
                }
            }
            catch
            {
                TempData["Error"] = "An error occurred while deleting the employee.";

                return RedirectToAction("Booking", "Booking", new { area = "Admin" });
            }



            return RedirectToAction("Booking", "Booking", new { area = "Admin" });
        }


        public async Task LoadDropDownList()
        {
            var client = GetApiClint();

            var Bacground =
                await client.GetFromJsonAsync<List<BackgroundDropdownDto>>(
                    "api/Booking/GetBachground");

            var RoseColor =
                await client.GetFromJsonAsync<List<RoseColorDropdownDto>>(
                    "api/Booking/GetRoseColor");

            var locations =
                await client.GetFromJsonAsync<List<LocationDropdownDto>>(
                    "api/Booking/GetLocations");

            var RoseType =
                await client.GetFromJsonAsync<List<RoseTypeDropdownDto>>(
                    "api/Booking/GetRoseType");

            var FrameColor =
                await client.GetFromJsonAsync<List<FrameColorDropdownDto>>(
                    "api/Booking/GetFrameColor");

            var services =
                await client.GetFromJsonAsync<List<ServiceDropdownDto>>(
                    "api/Booking/GetServices");


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
