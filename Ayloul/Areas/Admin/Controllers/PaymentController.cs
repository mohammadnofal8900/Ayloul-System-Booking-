using Ayloul.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Ayloul.Areas.Admin.Controllers
{
    [Authorize(Roles = "Super Admin,Admin")]
    [Area("Admin")]
    [Authorize]
    public class PaymentController : Controller
    {

        private readonly ApplicationDbContext _Context;


        public PaymentController(ApplicationDbContext Context)
        {
            _Context = Context;
        }
        [HttpGet]
        public async Task<IActionResult> Payment()
        {
            var payment = await _Context.Payments.Include(b=>b.Booking).ToListAsync();
            return View(payment);
        }
    }
}
