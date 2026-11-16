using Ayloul.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Threading.Tasks;

namespace Ayloul.Areas.Admin.Controllers
{
    [Authorize(Roles = "Super Admin")]
    [Area("Admin")]
    [Authorize]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        //View Role
        [HttpGet]
        public async Task<IActionResult> ViewRole()
        {
            var role = await _roleManager.Roles.ToListAsync();
            return View(role);
        }

        //Edit Role
        [HttpGet]
        public async Task<IActionResult> EditRole(string id)
        {
            var data = await _roleManager.FindByIdAsync(id);
            if (data == null)
            {
                ModelState.AddModelError("", "This Role Is Not Found .");
            }
            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditRole(string id,string RoleName)
        {
            var data=await _roleManager.FindByIdAsync(id);
            if (data == null)
            {
                ModelState.AddModelError("", "This Role Is Not Found .");
                return View(data);
            }
            if (string.IsNullOrWhiteSpace(RoleName))
            {
                ModelState.AddModelError("", "The Role Name is Required . ");
                return View(data);
            }
            RoleName = RoleName.Trim();
            if (await _roleManager.RoleExistsAsync(RoleName))
            {
                ModelState.AddModelError("", "The Role is already Exists . ");
                return View(data);
            }
            data.Name = RoleName;
            var result = await _roleManager.UpdateAsync(data);
            
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(ViewRole));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return RedirectToAction(nameof(ViewRole));
        }

        //Create Role

        [HttpGet]
        public async Task<IActionResult> CreateRole()
        {
            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRole(string RoleName)
        {
            if(string.IsNullOrWhiteSpace(RoleName))
            {
                ModelState.AddModelError("", "The Role Name is Required . ");
                return View();
            }
            RoleName = RoleName.Trim();
            if (await _roleManager.RoleExistsAsync(RoleName))
            {
                ModelState.AddModelError("", "The Role is already Exists . ");
                return View();
            }
            var result = await _roleManager.CreateAsync(new IdentityRole(RoleName));
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(ViewRole));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return RedirectToAction(nameof(ViewRole));

        }

        //Delete Role
        [HttpGet]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            if (role == null)
            {
                return NotFound();
            }
            return View(role);
        }

        [ActionName("DeleteRole")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete_Role(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            if (role == null)
            {
                return NotFound();
            }

            var result = await _roleManager.DeleteAsync(role);

            if (result.Succeeded)
            {
                return RedirectToAction(nameof(ViewRole));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(role);
        }
    }
}
