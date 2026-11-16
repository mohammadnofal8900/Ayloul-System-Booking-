using Microsoft.AspNetCore.Mvc;

namespace Ayloul.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TestController : Controller
    {
        public IActionResult TestIndex()
        {
            return View();
        }
    }
}
