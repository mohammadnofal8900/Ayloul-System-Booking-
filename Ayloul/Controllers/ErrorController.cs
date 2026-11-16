using Microsoft.AspNetCore.Mvc;

namespace Ayloul.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Error()
        {
            return View();
        }
    }
}
