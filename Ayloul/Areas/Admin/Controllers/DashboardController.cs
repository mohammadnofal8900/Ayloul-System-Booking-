using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ayloul.Areas.Admin.Controllers
{
    [Authorize(Roles = "Super Admin,Admin")]
    [Area("Admin")]
    [Authorize]
    public class DashboardController : Controller
    {
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}
