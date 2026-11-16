using Ayloul.Areas.Admin.Models;
using Ayloul.Areas.Admin.Services;
using Ayloul.Data;
using Ayloul.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Ayloul.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class EmployeeApiController : Controller
    {

        private readonly ApiResponseHandler _apiResponseHandler;
        private readonly IHttpClientFactory _httpClientFactory;
        

        public EmployeeApiController( IHttpClientFactory httpClientFactory, ApiResponseHandler apiResponseHandler)
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
        public async Task<IActionResult> EmployeeView()
        {
            var client = GetApiClint();

            var response = await client.GetAsync("api/Employee");

            var errorResult = _apiResponseHandler.Handle(response,
                this
                );

            if (errorResult != null)
                return errorResult;

            var json = await response.Content.ReadAsStringAsync();

            Console.WriteLine(json);

            var services =
                await response.Content.ReadFromJsonAsync<List<EmployeeVm>>();

            return View(services);
        }

        [HttpGet]
        public async Task<IActionResult> CreateEmployee()
        {
            await LoadDropDownList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateEmployee(EmployeeCreateVm model)
        {
            if (!ModelState.IsValid)
            {
                await LoadDropDownList();
                return View(model);
            }

            try
            {
                var client = GetApiClint();
                

                var response = await client.PostAsJsonAsync(
                    "api/Employee",
                    model);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(EmployeeView));
                }

                var errorMessage =
                    await response.Content.ReadAsStringAsync();

                ModelState.AddModelError(
                    string.Empty,
                    errorMessage);

                await LoadDropDownList();

                return View(model);
            }
            catch (HttpRequestException)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Unable to connect to the API.");

                await LoadDropDownList();

                return View(model);
            }
        }


        [HttpGet]
        public async Task<IActionResult> EditEmployee(int id)
        {

            var client = GetApiClint();

            var response = await client.GetAsync($"api/Employee/{id}");

            var errorResult = _apiResponseHandler.Handle(response,
                this
                );

            if (errorResult != null)
                return errorResult;

            var employee =
    await response.Content.ReadFromJsonAsync<EmployeeEditVm>();
            await LoadDropDownList();
            return View(employee);
        }

        [HttpPost]
        public async Task<IActionResult> EditEmployee(EmployeeEditVm model,int id)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var client = GetApiClint();

                var json = JsonConvert.SerializeObject(model);

                var content = new StringContent(
                    json,
                    Encoding.UTF8,
                    "application/json"
                );

                var response = await client.PutAsync(
                    $"api/Employee/{id}",
                    content
                );

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(EmployeeView));
                }

                var errorResult = _apiResponseHandler.Handle(response, this);

                if (errorResult != null)
                    return errorResult;

                ModelState.AddModelError(
                    "",
                    "Failed to update employee."
                );

                return View(model);
            }
            catch (Exception)
            {
                ModelState.AddModelError(
                    "",
                    "An error occurred while updating the employee."
                );

                return View(model);
            }

        }

        [HttpPost]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            try
            {
                var client = GetApiClint();

                var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(currentUserId))
                {
                    TempData["Error"] = "Unable to identify current user.";
                    return RedirectToAction("Employee");
                }

                client.DefaultRequestHeaders.Remove("CurrentUserId");
                client.DefaultRequestHeaders.Add("CurrentUserId", currentUserId);

                var response = await client.DeleteAsync($"api/Employee/{id}");

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(EmployeeView));
                }

                var errorResult = _apiResponseHandler.Handle(response, this);

                if (errorResult != null)
                    return errorResult;

                TempData["Error"] = "Failed to delete employee.";

                return RedirectToAction("Employee");
            }
            catch (Exception)
            {
                TempData["Error"] = "An error occurred while deleting the employee.";

                return RedirectToAction("Employee");
            }
        }
        public async Task LoadDropDownList()
        {
            var client = GetApiClint();
            

            var departments =
                await client.GetFromJsonAsync<List<DepartmentDropdownDto>>(
                    "api/Employee/GetDepartments");

            var genders =
                await client.GetFromJsonAsync<List<GenderDropdownDto>>(
                    "api/Employee/GetGenders");

            var locations =
                await client.GetFromJsonAsync<List<LocationDropdownDto>>(
                    "api/Employee/GetLocations");

            var roles =
                await client.GetFromJsonAsync<List<string>>(
                    "api/Employee/GetRoles");


            ViewBag.Department = new SelectList(
                departments,
                "DepartmentId",
                "DepartmentName");

            ViewBag.Gender = new SelectList(
                genders,
                "GenderId",
                "GenderName");

            ViewBag.Location = new SelectList(
                locations,
                "LocationId",
                "City");

            ViewBag.Role = new SelectList(roles);
        }
    }
}
