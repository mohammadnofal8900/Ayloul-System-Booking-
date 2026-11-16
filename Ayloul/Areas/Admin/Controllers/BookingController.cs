using Ayloul.Data;
using Ayloul.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Ayloul.Areas.Admin.Controllers
{
    [Authorize(Roles = "Super Admin,Admin")]
    [Area("Admin")]
    [Authorize]
    public class BookingController : Controller
    {
        private readonly ApplicationDbContext _Context;
        private readonly UserManager<ApplicationUser> _UserManager;
        private readonly IWebHostEnvironment _environment;

        public BookingController(ApplicationDbContext Context, UserManager<ApplicationUser> UserManager, IWebHostEnvironment environment) 
        {
            _Context = Context;
            _UserManager = UserManager;
            _environment = environment;
        }

        public IActionResult Booking()
        {
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> ViewBooking(Booking.BookingStatus status)
        {
            var bookings = await _Context.Bookings
                .Where(b => b.bookingStatus == status)
                .Include(b => b.Customer)
                    .ThenInclude(c => c.User)
                .Include(b => b.Location)
                .Include(b => b.Background)
                .Include(b => b.FrameColor)
                .Include(b => b.RoseType)
                .Include(b => b.RoseColor1)
                .Include(b => b.RoseColor2)
                .ToListAsync();

            LoadDropDownList();
            return View(bookings);
        }


        [HttpGet]
        public async Task<IActionResult> ViewInfoBooking(int id)
        {
            var info = await _Context.Bookings
        .Include(b => b.Customer)
        .Include(b => b.BookingServices).ThenInclude(bs => bs.Service)
        .Include(b => b.Location)
        .Include(b => b.Background)
        .Include(b => b.FrameColor)
        .Include(b => b.RoseType)
        .Include(b => b.RoseColor1)
        .Include(b => b.RoseColor2)
        .FirstOrDefaultAsync(b => b.Booking_Id == id);
            if (info == null)
                return NotFound();
            LoadDropDownList();
            return View(info);
        }

       

        [HttpGet]
        public async Task<IActionResult> EditBooking(int id)
        {

            var booking = await _Context.Bookings
        .Include(b => b.Customer)
        .Include(b => b.Location)
        .Include(b => b.Background)
        .Include(b => b.FrameColor)
        .Include(b => b.RoseType)
        .Include(b => b.RoseColor1)
        .Include(b => b.RoseColor2)
        .FirstOrDefaultAsync(b => b.Booking_Id == id);


            if (booking == null)
            {
               return NotFound();
            }
            LoadDropDownList();

            return View(booking);
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditBooking(Booking model)
        {
            if (!ModelState.IsValid)
            {
                LoadDropDownList();
                return View(model);
            }

            var book = await _Context.Bookings.FindAsync(model.Booking_Id);

            if (book == null)
            {
                return NotFound();
            }

            // البيانات الأساسية
            book.Customer_Id = model.Customer_Id;
            book.Name = model.Name;
            book.Description = model.Description;
            book.Event_Date = model.Event_Date;
            book.Date_Time = model.Date_Time;
            book.Note = model.Note;
            book.Phone_Number = model.Phone_Number;
            book.Is_Active=model.Is_Active;

            // الموقع والتصميم
            book.Location_Id = model.Location_Id;
            book.Background_Id = model.Background_Id;
            book.FrameColor_Id = model.FrameColor_Id;
            book.RoseType_Id = model.RoseType_Id;
            book.RoseColor1_Id = model.RoseColor1_Id;
            book.RoseColor2_Id = model.RoseColor2_Id;

            // الحالة
            book.bookingStatus = model.bookingStatus;

            // إذا كنت تسمح بتعديل حالة التفعيل
            book.Is_Active = model.Is_Active;

            // إذا كان السعر يحسب تلقائياً فأعد حسابه هنا
            // book.TotalPrice = ...;

            // رفع صورة جديدة
            if (model.ImageFile != null)
            {
                // حذف الصورة القديمة
                if (!string.IsNullOrEmpty(book.Note_Image))
                {
                    var oldImage = Path.Combine(_environment.WebRootPath, book.Note_Image.TrimStart('/'));

                    if (System.IO.File.Exists(oldImage))
                    {
                        System.IO.File.Delete(oldImage);
                    }
                }

                // إنشاء اسم جديد
                string fileName = Guid.NewGuid() + Path.GetExtension(model.ImageFile.FileName);

                string folder = Path.Combine(_environment.WebRootPath, "Images", "Notes");

                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                string filePath = Path.Combine(folder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ImageFile.CopyToAsync(stream);
                }

                book.Note_Image = "/Images/Notes/" + fileName;
            }

            await _Context.SaveChangesAsync();

            TempData["success"] = "Booking updated successfully.";

            return RedirectToAction("Booking", "Booking", new { area = "Admin" });
        }










        [HttpGet]
        public async Task<IActionResult> DeleteBooking(int Id)
        {
            var booking = await _Context.Bookings
                .Include(b => b.Location)
                .Include(b => b.Background)
                .Include(b => b.FrameColor)
                .Include(b => b.RoseType)
                .Include(b => b.RoseColor1)
                .Include(b => b.RoseColor2)
                .FirstOrDefaultAsync(b => b.Booking_Id == Id);


            if (booking == null)
            {
                return NotFound();
            }
            ViewBag.Department = new SelectList(
                await _Context.Departments.ToListAsync(),
                "Department_Id",
                "Department_Name");

            ViewBag.Gender = new SelectList(
                await _Context.Genders.ToListAsync(),
                "Gender_Id",
                "Gender_Name");

            ViewBag.Location = new SelectList(
                await _Context.Locations.ToListAsync(),
                "Location_Id",
                "City");

            ViewBag.Backgrounds = new SelectList(
                await _Context.Backgrounds.ToListAsync(),
                "Background_Id",
                "Background_Color");

            ViewBag.FrameColors = new SelectList(
                await _Context.FrameColors.ToListAsync(),
                "FrameColor_Id",
                "FrameColor_Color");

            ViewBag.RoseTypes = new SelectList(
                await _Context.RoseTypes.ToListAsync(),
                "RoseType_Id",
                "RoseType_Name");

            ViewBag.RoseColors = new SelectList(
                await _Context.RoseColors.ToListAsync(),
                "RoseColor_Id",
                "RoseColor_Name");

            ViewBag.Services = await _Context.Services
                .Where(s => s.Is_Active)
                .ToListAsync();
        
            return View(booking);
        }

        [HttpPost]
        [ActionName("DeleteBooking")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteBookingConfirmed(int Id)
        {
            var booking = await _Context.Bookings.FindAsync(Id);

            if (booking == null)
            {
                return NotFound();
            }

            var user = await _UserManager.GetUserAsync(User);

            if (user == null)
            {
                return Content("User is null");
            }

            await _Context.Database.OpenConnectionAsync();

            await _Context.Database.ExecuteSqlRawAsync(
                "EXEC sp_set_session_context @key=N'UserName', @value={0}",
                user.UserName
            );

            _Context.Bookings.Remove(booking);

            await _Context.SaveChangesAsync();

            return RedirectToAction("Booking", "Booking", new { area = "Admin" });
        }


        private async Task LoadDropDownList()
        {
            ViewBag.Department = new SelectList(
                await _Context.Departments.ToListAsync(),
                "Department_Id",
                "Department_Name");

            ViewBag.Gender = new SelectList(
                await _Context.Genders.ToListAsync(),
                "Gender_Id",
                "Gender_Name");

            ViewBag.Location = new SelectList(
                await _Context.Locations.ToListAsync(),
                "Location_Id",
                "City");

            ViewBag.Backgrounds = new SelectList(
                await _Context.Backgrounds.ToListAsync(),
                "Background_Id",
                "Background_Color");

            ViewBag.FrameColors = new SelectList(
                await _Context.FrameColors.ToListAsync(),
                "FrameColor_Id",
                "FrameColor_Color");

            ViewBag.RoseTypes = new SelectList(
                await _Context.RoseTypes.ToListAsync(),
                "RoseType_Id",
                "RoseType_Name");

            ViewBag.RoseColors = new SelectList(
                await _Context.RoseColors.ToListAsync(),
                "RoseColor_Id",
                "RoseColor_Name");

            ViewBag.Services = await _Context.Services
                .Where(s => s.Is_Active)
                .ToListAsync();
        }


    }
}
