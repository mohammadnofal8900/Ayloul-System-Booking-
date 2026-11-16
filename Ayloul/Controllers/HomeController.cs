using System.Diagnostics;
using Ayloul.Models;
using Microsoft.AspNetCore.Mvc;

namespace Ayloul.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Error404()
        {
            return View();
        }
        public IActionResult TestHome()
        {
            return View();
        }
        public IActionResult TestHome2()
        {
            return View();
        }




    }
}
