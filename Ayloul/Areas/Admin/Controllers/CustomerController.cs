using Ayloul.Data;
using Ayloul.Models;
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
    public class CustomerController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        
        private readonly ApplicationDbContext _context;

        public CustomerController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
            

        }
        [HttpGet]
        public async Task<IActionResult> ViewCustomer()
        {
            var users = await _context.Users
    .Where(u => !_context.UserRoles
        .Any(ur => ur.UserId == u.Id))
    .ToListAsync();

            return View(users);


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeStatus(string id, bool isActive)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
                return NotFound();

            user.Is_Active = isActive;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                TempData["Error"] = "Failed to update status.";
            }

            return RedirectToAction(nameof(ViewCustomer)); // أو اسم صفحة عرض العملاء
        }


        
    }
}
