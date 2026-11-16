using Ayloul.Data;
using Ayloul.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.Intrinsics.Arm;
using System.Threading.Tasks;

namespace Ayloul.Areas.Admin.Controllers
{
    [Authorize(Roles = "Super Admin")]
    [Area("Admin")]
    [Authorize]
    public class DepartmentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DepartmentController(ApplicationDbContext context) 
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> ViewDepartment()
        {
            
            var result=await _context.Departments.ToListAsync();

            return View(result);
        }
        [HttpGet]
        public async Task<IActionResult> AddDepartment()
        {
            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddDepartment(Department model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var depExists = await _context.Departments
                .AnyAsync(d => d.Department_Name == model.Department_Name);

            if (depExists)
            {
                ModelState.AddModelError("", "This Department already exists.");
                return View(model);
            }

            var dep = new Department
            {
                Department_Name = model.Department_Name
            };

            _context.Departments.Add(dep);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(ViewDepartment));
        }

        [HttpGet]
        public async Task<IActionResult> EditDepartment(int id)
        {
            var dep = await _context.Departments.FindAsync(id);

            if (dep == null)
                return NotFound();

            return View(dep);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditDepartment(Department model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var dep = await _context.Departments.FindAsync(model.Department_Id);

            if (dep == null)
                return NotFound();

            var depExists = await _context.Departments
                .AnyAsync(d => d.Department_Name == model.Department_Name
                            && d.Department_Id != model.Department_Id);

            if (depExists)
            {
                ModelState.AddModelError("", "This Department already exists.");
                return View(model);
            }

            dep.Department_Name = model.Department_Name;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(ViewDepartment));
        }

        [HttpGet]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            var dep = await _context.Departments.FindAsync(id);

            if (dep == null)
                return NotFound();

            return View(dep);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteDepartment(Department model)
        {
            var dep = await _context.Departments.FindAsync(model.Department_Id);

            if (dep == null)
            {
                return NotFound();
            }

            _context.Departments.Remove(dep);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(ViewDepartment));

        }
    }
}
