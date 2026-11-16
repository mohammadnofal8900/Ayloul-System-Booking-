//using Ayloul.Areas.Admin.Models;
//using Ayloul.Data;
//using Ayloul.Models;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using Microsoft.EntityFrameworkCore;
//using System.Threading.Tasks;

//namespace Ayloul.Areas.Admin.Controllers
//{
//    [Authorize(Roles = "Super Admin")]
//    [Area("Admin")]
//    public class EmployeeController : Controller
//    {
//        private readonly UserManager<ApplicationUser> _userManager;
//        private readonly RoleManager<IdentityRole> _roleManager;
//        private readonly ApplicationDbContext _context;

//        public EmployeeController(ApplicationDbContext context, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
//        {
//            _context = context;
//            _userManager = userManager;
//            _roleManager = roleManager;

//        }

//        [HttpGet]
//        public async Task<IActionResult> ViewEmployee()
//        {
//            var employees = await _context.Employees.Include(d => d.department).Include(u => u.User).Include(g => g.Gender).Include(l => l.Location).ToListAsync();
//            var roles = new Dictionary<string, string>();

//            foreach (var employee in employees)
//            {
//                var userRoles = await _userManager.GetRolesAsync(employee.User);

//                roles.Add(employee.UserId, userRoles.FirstOrDefault() ?? "No Role");
//            }

//            ViewBag.Roles = roles;

//            return View(employees);
//        }

//        [HttpGet]
//        public IActionResult CreateEmployee()
//        {
//            LoadDropDownList();
//            return View();
//        }

//        [HttpPost]
//        public async Task<IActionResult> CreateEmployee(EmployeeCreateVm model)
//        {
//            if (!ModelState.IsValid)
//            {
//                LoadDropDownList();
//                return View(model);
//            }

//            // البحث عن المستخدم بالبريد الإلكتروني
//            var user = await _userManager.FindByEmailAsync(model.Employee_Email);

//            // إذا لم يكن موجودًا، أنشئ مستخدمًا جديدًا
//            if (user == null)
//            {
//                user = new ApplicationUser
//                {
//                    FUllName = model.Full_Name,
//                    Email = model.Employee_Email,
//                    UserName = model.Employee_Email,
//                    PhoneNumber = model.Mobile_Phone,
//                    Is_Active = true
//                };

//                var result = await _userManager.CreateAsync(user);

//                if (!result.Succeeded)
//                {
//                    foreach (var item in result.Errors)
//                    {
//                        ModelState.AddModelError("", item.Description);
//                    }

//                    LoadDropDownList();
//                    return View(model);
//                }
//            }

//            // التحقق إذا كان هذا المستخدم موظفًا بالفعل
//            var employeeExists = await _context.Employees
//                .AnyAsync(e => e.UserId == user.Id);
            

//            if (employeeExists)
//            {
//                ModelState.AddModelError("", "This employee already exists.");

//                LoadDropDownList();
//                return View(model);
//            }

//            var locationExists = await _context.Locations
//    .AnyAsync(l => l.Location_Id == model.Location_Id);

//            Console.WriteLine($"Location_Id = {model.Location_Id}");
//            Console.WriteLine($"Exists = {locationExists}");
//            // إنشاء سجل الموظف
//            Employee employee = new Employee
//            {
//                UserId = user.Id,
//                Department_Id = model.Department_Id,
//                Gender_Id = model.Gender_Id,
//                Location_Id = model.Location_Id,
//                Employee_Salary = model.Employee_Salary,
//                Mobile_Phone = model.Mobile_Phone,
//                Employee_Email = model.Employee_Email,
//                Employee_Date = model.Employee_Date,
//                Is_Active = true
//            };
//            // إعطاء المستخدم الـ Role المختار
//            if (!string.IsNullOrEmpty(model.RoleName))
//            {
//                if (!await _userManager.IsInRoleAsync(user, model.RoleName))
//                {
//                    await _userManager.AddToRoleAsync(user, model.RoleName);
//                }
//            }

//            Console.WriteLine($"Location_Id = {model.Location_Id}");

//            bool exists = await _context.Locations
//                .AnyAsync(x => x.Location_Id == model.Location_Id);

//            Console.WriteLine($"Exists = {exists}");
//            _context.Employees.Add(employee);
//            await _context.SaveChangesAsync();

//            return RedirectToAction(nameof(ViewEmployee));
//        }


//            [HttpGet]
//            public async Task<IActionResult> EditEmployee(string id)
//            {
//                if (id == null)
//                    return NotFound();

//                var employee = await _context.Employees
//                    .Include(e => e.User)
//                    .FirstOrDefaultAsync(x => x.UserId == id);

//                if (employee == null)
//                    return NotFound();


//                var user = await _userManager.FindByIdAsync(employee.UserId);

//                var roles = new List<string>();

//                if (user != null)
//                {
//                    roles = (await _userManager.GetRolesAsync(user)).ToList();
//                }


//                ViewBag.Department = new SelectList(
//                    _context.Departments,
//                    "Department_Id",
//                    "Department_Name",
//                    employee.Department_Id
//                );


//                ViewBag.Gender = new SelectList(
//                    _context.Genders,
//                    "Gender_Id",
//                    "Gender_Name",
//                    employee.Gender_Id
//                );


//                ViewBag.Location = new SelectList(
//                    _context.Locations,
//                    "Location_Id",
//                    "City",
//                    employee.Location_Id
//                );


//                ViewBag.Role = new SelectList(
//                    _roleManager.Roles.ToList(),
//                    "Name",
//                    "Name",
//                    roles.FirstOrDefault()
//                );


//            var model = new EmployeeEditVm
//            {
//                UserId = employee.UserId,
//                Full_Name = employee.User.FUllName,
//                Employee_Email = employee.Employee_Email,
//                Employee_Salary = employee.Employee_Salary,
//                Mobile_Phone = employee.Mobile_Phone,
//                Employee_Date = employee.Employee_Date,
//                Is_Active = employee.Is_Active,

//                Department_Id = employee.Department_Id,
//                Gender_Id = employee.Gender_Id,
//                Location_Id = employee.Location_Id,

//                RoleName = roles.FirstOrDefault()
//            };


//            return View(model);
//        }

//        [HttpPost]
//        public async Task<IActionResult> EditEmployee(EmployeeEditVm model)
//        {
//            if (!ModelState.IsValid)
//            {
//                LoadDropDownList();
//                return View(model);
//            }


//            var employee = await _context.Employees
//                .FirstOrDefaultAsync(e => e.UserId == model.UserId);



//            if (employee == null)
//                return NotFound();


//            employee.Employee_Email = model.Employee_Email;
//            employee.Employee_Salary = model.Employee_Salary;
//            employee.Mobile_Phone = model.Mobile_Phone;
//            employee.Employee_Date = model.Employee_Date;
//            employee.Is_Active = model.Is_Active;

//            employee.Department_Id = model.Department_Id;
//            employee.Gender_Id = model.Gender_Id;
//            employee.Location_Id = model.Location_Id;


//            _context.Update(employee);
//            await _context.SaveChangesAsync();


//            var user = await _userManager.FindByIdAsync(model.UserId);


//            if (user != null)
//            {
//                user.FUllName = model.Full_Name;
//                user.Email = model.Employee_Email;
//                user.UserName = model.Employee_Email;
//                user.PhoneNumber = model.Mobile_Phone;

//                await _userManager.UpdateAsync(user);


//                // تعديل الـ Role
//                var oldRoles = await _userManager.GetRolesAsync(user);

//                await _userManager.RemoveFromRolesAsync(user, oldRoles);


//                if (!string.IsNullOrEmpty(model.RoleName))
//                {
//                    await _userManager.AddToRoleAsync(user, model.RoleName);
//                }
//            }


//            return RedirectToAction(nameof(ViewEmployee));
//        }

//        [HttpGet]
//        public async Task<IActionResult> DeleteEmployee(string id)
//        {
//            if (id == null)
//                return NotFound();


//            var employee = await _context.Employees
//                .Include(e => e.User)
//                .Include(e => e.department)
//                .Include(e => e.Gender)
//                .Include(e => e.Location)
//                .FirstOrDefaultAsync(e => e.UserId == id);


//            if (employee == null)
//                return NotFound();


//            return View(employee);
//        }

//        [HttpPost, ActionName("DeleteEmployee")]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> DeleteConfirmed(string id)
//        {

//            var employee = await _context.Employees
//                .FirstOrDefaultAsync(e => e.UserId == id);


//            if (employee == null)
//                return NotFound();



//            var user = await _userManager.FindByIdAsync(id);


//            if (user == null)
//                return NotFound();

//            var DeletedBy = _userManager.GetUserId(User);


//            await _context.Database.OpenConnectionAsync();

//            await _context.Database.ExecuteSqlRawAsync(
//                "EXEC sp_set_session_context @key=N'CurrentUserId', @value={0}",
//                DeletedBy
//            );



//            // 1- حذف الموظف
//            _context.Employees.Remove(employee);

//            await _context.SaveChangesAsync();



//            // 2- إزالة كل الصلاحيات الحالية
//            var roles = await _userManager.GetRolesAsync(user);

//            await _userManager.RemoveFromRolesAsync(user, roles);



//            //// 3- إضافة Customer Role
//            //await _userManager.AddToRoleAsync(user, "Customer");



//            // 4- إضافة إلى جدول Customers

//            var customerExists = await _context.Customers
//                .AnyAsync(c => c.UserId == user.Id);


//            if (!customerExists)
//            {
//                var customer = new Customer
//                {
//                    UserId = user.Id,
//                    //CreatedDate = DateTime.Now
//                };


//                _context.Customers.Add(customer);

//                await _context.SaveChangesAsync();
//            }



//            return RedirectToAction(nameof(ViewEmployee));
//        }

//        public void LoadDropDownList()
//        {
//            ViewBag.Department = new SelectList(_context.Departments, "Department_Id", "Department_Name");
//            ViewBag.Gender = new SelectList(_context.Genders, "Gender_Id", "Gender_Name");
//            ViewBag.Location = new SelectList(_context.Locations, "Location_Id", "City");
//            ViewBag.Role = new SelectList(_context.Roles.ToList(), "Name", "Name");
//        }
//    }
//}
