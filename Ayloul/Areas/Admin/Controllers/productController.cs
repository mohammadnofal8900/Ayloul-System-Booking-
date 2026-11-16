using Ayloul.Data;
using Ayloul.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Ayloul.Areas.Admin.Controllers
{
    [Authorize(Roles = "Super Admin")]
    [Area("Admin")]
    [Authorize]
    public class productController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public productController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        //View All Product   =======================================================================

        [HttpGet]
        public IActionResult ViewProduct()
        {
            return View();
        }

        //ROSE TYPE   =======================================================================

        [HttpGet]
        public async Task<IActionResult> ViewRoseType()
        {
            var rosetype = await _context.RoseTypes.ToListAsync();
            return View(rosetype);
        }
        [HttpGet]
        public IActionResult AddRoseType()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddRoseType(RoseType model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var RosExists = await _context.RoseTypes.AnyAsync(rt => rt.RoseType_Name == model.RoseType_Name);

            if (RosExists)
            {
                ModelState.AddModelError("", "This Rose Type already exists.");
                return View(model);
            }

            var rosetype = new RoseType
            {
                RoseType_Name = model.RoseType_Name,
                RoseType_Description = model.RoseType_Description,
                
            };

            _context.RoseTypes.Add(rosetype);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(ViewRoseType));
        }

        [HttpGet]
        public async Task<IActionResult> EditRoseType(int id)
        {
            var ros = await _context.RoseTypes.FindAsync(id);

            if (ros == null)
                return NotFound();

            return View(ros);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditRoseType(RoseType model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var ros = await _context.RoseTypes.FindAsync(model.RoseType_Id);

            if (ros == null)
                return NotFound();

            var rosExists = await _context.RoseTypes
                .AnyAsync(d => d.RoseType_Name == model.RoseType_Name
                            && d.RoseType_Id != model.RoseType_Id);

            if (rosExists)
            {
                ModelState.AddModelError("", "This Rose Type already exists.");
                return View(model);
            }

            ros.RoseType_Name = model.RoseType_Name;
            ros.RoseType_Description = model.RoseType_Description;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(ViewRoseType));
        }

        [HttpGet]
        public async Task<IActionResult> DeleteRoseType(int id)
        {
            var ros = await _context.RoseTypes.FindAsync(id);

            if (ros == null)
                return NotFound();

            return View(ros);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteRoseType(RoseType model)
        {
            var ros = await _context.RoseTypes.FindAsync(model.RoseType_Id);

            if (ros == null)
            {
                return NotFound();
            }

            _context.RoseTypes.Remove(ros);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(ViewRoseType));

        }

        //End Rose Type

        //Rose Color   =======================================================================
        [HttpGet]
        public async Task<IActionResult> ViewRoseColor()
        {
            var rosecolor = await _context.RoseColors.ToListAsync();
            return View(rosecolor);
        }

        [HttpGet]
        public IActionResult AddRoseColor()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddRoseColor(RoseColor model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var RosExists = await _context.RoseColors.AnyAsync(rt => rt.RoseColor_Name == model.RoseColor_Name);

            if (RosExists)
            {
                ModelState.AddModelError("", "This Rose Color already exists.");
                return View(model);
            }

            if (model.ImageFile != null)
            {
                string originalName = Path.GetFileNameWithoutExtension(model.ImageFile.FileName);

                string extension = Path.GetExtension(model.ImageFile.FileName);
                string fileName = originalName
                  + "_"
                  + DateTime.Now.ToString("yyyyMMdd_HHmmss")
                  + extension;

                string folder = Path.Combine(
                    _environment.WebRootPath,
                    "images",
                    "Rose_Color"
                    );

                // إنشاء المجلد إذا لم يكن موجوداً
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                string filePath =
                    Path.Combine(folder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ImageFile.CopyToAsync(stream);
                }

                model.img = fileName;
            }

            _context.RoseColors.Add(model);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(ViewRoseColor));
        }

        [HttpGet]
        public async Task<IActionResult> EditRoseColor(int id)
        {
            var roseColor = await _context.RoseColors
                .FirstOrDefaultAsync(x => x.RoseColor_Id == id);

            if (roseColor == null)
                return NotFound();

            return View(roseColor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditRoseColor(RoseColor model)
        {
            if (!ModelState.IsValid)
                return View(model);


            var roseColor = await _context.RoseColors
                .FirstOrDefaultAsync(x => x.RoseColor_Id == model.RoseColor_Id);


            if (roseColor == null)
                return NotFound();


            var nameExists = await _context.RoseColors
                .AnyAsync(x => x.RoseColor_Name == model.RoseColor_Name
                && x.RoseColor_Id != model.RoseColor_Id);


            if (nameExists)
            {
                ModelState.AddModelError("", "This Rose Color already exists.");
                return View(model);
            }


            // إذا المستخدم رفع صورة جديدة
            if (model.ImageFile != null)
            {

                // حذف الصورة القديمة
                if (!string.IsNullOrEmpty(roseColor.img))
                {
                    string oldPath = Path.Combine(
                        _environment.WebRootPath,
                        "Images",
                        "Rose_Color",
                        roseColor.img
                    );

                    if (System.IO.File.Exists(oldPath))
                    {
                        System.IO.File.Delete(oldPath);
                    }
                }


                string originalName =
                    Path.GetFileNameWithoutExtension(model.ImageFile.FileName);

                string extension =
                    Path.GetExtension(model.ImageFile.FileName);


                string fileName =
                    originalName
                    + "_"
                    + DateTime.Now.ToString("yyyyMMdd_HHmmss")
                    + extension;


                string folder =
                    Path.Combine(_environment.WebRootPath, "Images","Rose_Color");


                string filePath =
                    Path.Combine(folder, fileName);


                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ImageFile.CopyToAsync(stream);
                }


                roseColor.img = fileName;
            }


            // تحديث البيانات
            roseColor.RoseColor_Name = model.RoseColor_Name;


            await _context.SaveChangesAsync();


            return RedirectToAction(nameof(ViewRoseColor));
        }

        [HttpGet]
        public async Task<IActionResult> DeleteRoseColor(int id)
        {
            var roseColor = await _context.RoseColors
                .FirstOrDefaultAsync(x => x.RoseColor_Id == id);


            if (roseColor == null)
                return NotFound();


            return View(roseColor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteRoseColor(RoseColor model)
        {
            var roseColor = await _context.RoseColors
                .FirstOrDefaultAsync(x => x.RoseColor_Id == model.RoseColor_Id);


            if (roseColor == null)
                return NotFound();


            // حذف الصورة
            if (!string.IsNullOrEmpty(roseColor.img))
            {
                string imagePath = Path.Combine(
                    _environment.WebRootPath,
                    "Images",
                    roseColor.img
                );


                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }


            _context.RoseColors.Remove(roseColor);

            await _context.SaveChangesAsync();


            return RedirectToAction(nameof(ViewRoseColor));
        }

        //End Rose Color

        //Background   =======================================================================

        [HttpGet]
        public async Task<IActionResult> ViewBackground()
        {
            var background = await _context.Backgrounds.ToListAsync();
            return View(background);
        }

        [HttpGet]
        public IActionResult AddBackground()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddBackground(Background model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var BackExists = await _context.Backgrounds.AnyAsync(ba => ba.Background_Color == model.Background_Color);

            if (BackExists)
            {
                ModelState.AddModelError("", "This Color already exists.");
                return View(model);
            }

            if (model.ImageFile != null)
            {
                string originalName = Path.GetFileNameWithoutExtension(model.ImageFile.FileName);

                string extension = Path.GetExtension(model.ImageFile.FileName);
                string fileName = originalName
                  + "_"
                  + DateTime.Now.ToString("yyyyMMdd_HHmmss")
                  + extension;

                string folder = Path.Combine(
                    _environment.WebRootPath,
                    "images",
                    "Background_Color"
                    );

                // إنشاء المجلد إذا لم يكن موجوداً
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                string filePath =
                    Path.Combine(folder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ImageFile.CopyToAsync(stream);
                }

                model.img = fileName;
            }

            _context.Backgrounds.Add(model);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(ViewBackground));
        }

        [HttpGet]
        public async Task<IActionResult> EditBackground(int id)
        {
            var background = await _context.Backgrounds
                .FirstOrDefaultAsync(x => x.Background_Id == id);

            if (background == null)
                return NotFound();

            return View(background);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditBackground(Background model)
        {
            if (!ModelState.IsValid)
                return View(model);


            var backgroundcolor = await _context.Backgrounds
                .FirstOrDefaultAsync(x => x.Background_Id == model.Background_Id);


            if (backgroundcolor == null)
                return NotFound();


            var nameExists = await _context.Backgrounds
                .AnyAsync(x => x.Background_Color == model.Background_Color
                && x.Background_Id != model.Background_Id);


            if (nameExists)
            {
                ModelState.AddModelError("", "This Color already exists.");
                return View(model);
            }


            // إذا المستخدم رفع صورة جديدة
            if (model.ImageFile != null)
            {

                // حذف الصورة القديمة
                if (!string.IsNullOrEmpty(backgroundcolor.img))
                {
                    string oldPath = Path.Combine(
                        _environment.WebRootPath,
                        "Images",
                        "Background_Color",
                        backgroundcolor.img
                    );

                    if (System.IO.File.Exists(oldPath))
                    {
                        System.IO.File.Delete(oldPath);
                    }
                }


                string originalName =
                    Path.GetFileNameWithoutExtension(model.ImageFile.FileName);

                string extension =
                    Path.GetExtension(model.ImageFile.FileName);


                string fileName =
                    originalName
                    + "_"
                    + DateTime.Now.ToString("yyyyMMdd_HHmmss")
                    + extension;


                string folder =
                    Path.Combine(_environment.WebRootPath, "Images", "Background_Color");


                string filePath =
                    Path.Combine(folder, fileName);


                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ImageFile.CopyToAsync(stream);
                }


                backgroundcolor.img = fileName;
            }


            // تحديث البيانات
            backgroundcolor.Background_Color = model.Background_Color;


            await _context.SaveChangesAsync();


            return RedirectToAction(nameof(ViewBackground));
        }

        [HttpGet]
        public async Task<IActionResult> DeleteBackground(int id)
        {
            var background = await _context.Backgrounds
                .FirstOrDefaultAsync(x => x.Background_Id == id);


            if (background == null)
                return NotFound();


            return View(background);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteBackground(Background model)
        {
            var background = await _context.Backgrounds
                .FirstOrDefaultAsync(x => x.Background_Id == model.Background_Id);


            if (background == null)
                return NotFound();


            // حذف الصورة
            if (!string.IsNullOrEmpty(background.img))
            {
                string imagePath = Path.Combine(
                    _environment.WebRootPath,
                    "Images",    
                    "Background_Color",
                    background.img
                );


                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }


            _context.Backgrounds.Remove(background);

            await _context.SaveChangesAsync();


            return RedirectToAction(nameof(ViewBackground));
        }

        //End Add Background

        //Frame Color   =======================================================================


        [HttpGet]
        public async Task<IActionResult> ViewFrame()
        {
            var frame = await _context.FrameColors.ToListAsync();
            return View(frame);
        }

        [HttpGet]
        public IActionResult AddFrame()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddFrame(FrameColor model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var frameExists = await _context.FrameColors.AnyAsync(ba => ba.FrameColor_Color == model.FrameColor_Color);

            if (frameExists)
            {
                ModelState.AddModelError("", "This Color already exists.");
                return View(model);
            }

            if (model.ImageFile != null)
            {
                string originalName = Path.GetFileNameWithoutExtension(model.ImageFile.FileName);

                string extension = Path.GetExtension(model.ImageFile.FileName);
                string fileName = originalName
                  + "_"
                  + DateTime.Now.ToString("yyyyMMdd_HHmmss")
                  + extension;

                string folder = Path.Combine(
                    _environment.WebRootPath,
                    "images",
                    "Frame_Color"
                    );

                // إنشاء المجلد إذا لم يكن موجوداً
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                string filePath =
                    Path.Combine(folder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ImageFile.CopyToAsync(stream);
                }

                model.img = fileName;
            }

            _context.FrameColors.Add(model);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(ViewFrame));
        }

        [HttpGet]
        public async Task<IActionResult> EditFrame(int id)
        {
            var frame = await _context.FrameColors
                .FirstOrDefaultAsync(x => x.FrameColor_Id == id);

            if (frame == null)
                return NotFound();

            return View(frame);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditFrame(FrameColor model)
        {
            if (!ModelState.IsValid)
                return View(model);


            var framecolor = await _context.FrameColors
                .FirstOrDefaultAsync(x => x.FrameColor_Id == model.FrameColor_Id);


            if (framecolor == null)
                return NotFound();


            var nameExists = await _context.FrameColors
                .AnyAsync(x => x.FrameColor_Color == model.FrameColor_Color
                && x.FrameColor_Id != model.FrameColor_Id);


            if (nameExists)
            {
                ModelState.AddModelError("", "This Color already exists.");
                return View(model);
            }


            // إذا المستخدم رفع صورة جديدة
            if (model.ImageFile != null)
            {

                // حذف الصورة القديمة
                if (!string.IsNullOrEmpty(framecolor.img))
                {
                    string oldPath = Path.Combine(
                        _environment.WebRootPath,
                        "Images",
                        "Frame_Color",
                        framecolor.img
                    );

                    if (System.IO.File.Exists(oldPath))
                    {
                        System.IO.File.Delete(oldPath);
                    }
                }


                string originalName =
                    Path.GetFileNameWithoutExtension(model.ImageFile.FileName);

                string extension =
                    Path.GetExtension(model.ImageFile.FileName);


                string fileName =
                    originalName
                    + "_"
                    + DateTime.Now.ToString("yyyyMMdd_HHmmss")
                    + extension;


                string folder =
                    Path.Combine(_environment.WebRootPath, "Images", "Frame_Color");


                string filePath =
                    Path.Combine(folder, fileName);


                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ImageFile.CopyToAsync(stream);
                }

                
                framecolor.img = fileName;
            }


            // تحديث البيانات
            framecolor.FrameColor_Color = model.FrameColor_Color;


            await _context.SaveChangesAsync();


            return RedirectToAction(nameof(ViewFrame));
        }

        [HttpGet]
        public async Task<IActionResult> DeleteFrame(int id)
        {
            var frame = await _context.FrameColors
                .FirstOrDefaultAsync(x => x.FrameColor_Id == id);


            if (frame == null)
                return NotFound();


            return View(frame);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteFrame(FrameColor model)
        {
            var frame = await _context.FrameColors
                .FirstOrDefaultAsync(x => x.FrameColor_Id == model.FrameColor_Id);


            if (frame == null)
                return NotFound();


            // حذف الصورة
            if (!string.IsNullOrEmpty(frame.img))
            {
                string imagePath = Path.Combine(
                    _environment.WebRootPath,
                    "Images",
                    "Frame_Color",
                    frame.img
                );


                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }


            _context.FrameColors.Remove(frame);

            await _context.SaveChangesAsync();


            return RedirectToAction(nameof(ViewFrame));
        }

        //End Add Frame 

        //Service    =======================================================================

        [HttpGet]
        public async Task<IActionResult> ViewService()
        {
            var servic=await _context.Services.ToListAsync();
            return View(servic);
        }

        [HttpGet]
        public IActionResult AddService()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddService(Service model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var serviceExists = await _context.Services
                .AnyAsync(s => s.Service_Name == model.Service_Name);

            if (serviceExists)
            {
                ModelState.AddModelError("", "This Service already exists.");
                return View(model);
            }

            // رفع الصورة
            if (model.ImageFile != null)
            {
                string originalName = Path.GetFileNameWithoutExtension(model.ImageFile.FileName);
                string extension = Path.GetExtension(model.ImageFile.FileName);

                string fileName = originalName + "_"
                                + DateTime.Now.ToString("yyyyMMdd_HHmmss")
                                + extension;

                string folder = Path.Combine(
                    _environment.WebRootPath,
                    "images",
                    "Service");

                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }

                string filePath = Path.Combine(folder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ImageFile.CopyToAsync(stream);
                }

                model.img = fileName;
            }

            // إذا لم يتم اختيار الحالة من الـ View
            model.Is_Active = model.Is_Active;

            // الاسم والسعر موجودان داخل model تلقائياً
            // model.Service_Name
            // model.Price

            _context.Services.Add(model);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(ViewService));
        }

        [HttpGet]
        public async Task<IActionResult> EditService(int id)
        {
            if (id == 0)
                return NotFound();

            var service = await _context.Services.FindAsync(id);

            if (service == null)
                return NotFound();

            return View(service);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditService(Service model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var service = await _context.Services.FindAsync(model.Service_Id);

            if (service == null)
                return NotFound();

            var serviceExists = await _context.Services.AnyAsync(s =>
                s.Service_Name == model.Service_Name &&
                s.Service_Id != model.Service_Id);

            if (serviceExists)
            {
                ModelState.AddModelError("", "This Service already exists.");
                return View(model);
            }

            // تغيير الصورة إذا تم اختيار صورة جديدة
            if (model.ImageFile != null)
            {
                // حذف الصورة القديمة
                if (!string.IsNullOrEmpty(service.img))
                {
                    string oldImage = Path.Combine(
                        _environment.WebRootPath,
                        "images",
                        "Service",
                        service.img);

                    if (System.IO.File.Exists(oldImage))
                    {
                        System.IO.File.Delete(oldImage);
                    }
                }

                // حفظ الصورة الجديدة
                string originalName = Path.GetFileNameWithoutExtension(model.ImageFile.FileName);
                string extension = Path.GetExtension(model.ImageFile.FileName);

                string fileName = originalName + "_"
                                + DateTime.Now.ToString("yyyyMMdd_HHmmss")
                                + extension;

                string folder = Path.Combine(
                    _environment.WebRootPath,
                    "images",
                    "Service");

                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }

                string filePath = Path.Combine(folder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ImageFile.CopyToAsync(stream);
                }

                service.img = fileName;
            }

            // تحديث البيانات
            service.Service_Name = model.Service_Name;
            service.Price = model.Price;
            service.Is_Active = model.Is_Active;

            _context.Services.Update(service);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(ViewService));
        }

        [HttpGet]
        public async Task<IActionResult> DeleteService(int id)
        {
            if (id == 0)
                return NotFound();

            var service = await _context.Services.FindAsync(id);

            if (service == null)
                return NotFound();

            return View(service);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteService(Service model)
        {
            var service = await _context.Services.FindAsync(model.Service_Id);

            if (service == null)
                return NotFound();

            // حذف الصورة
            if (!string.IsNullOrEmpty(service.img))
            {
                string imagePath = Path.Combine(
                    _environment.WebRootPath,
                    "images",
                    "Service",
                    service.img);

                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }

            _context.Services.Remove(service);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(ViewService));
        }

    }
}
