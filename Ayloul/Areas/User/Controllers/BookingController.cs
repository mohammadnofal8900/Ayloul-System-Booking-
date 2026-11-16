//using Ayloul.Areas.User.Models;
//using Ayloul.Data;
//using Ayloul.Models;
//using DocumentFormat.OpenXml.InkML;
//using DocumentFormat.OpenXml.Spreadsheet;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using Microsoft.EntityFrameworkCore;
//using System.Threading.Tasks;
//using static Ayloul.Models.Booking;
//using static Ayloul.Models.Payment;

//namespace Ayloul.Areas.User.Controllers
//{
//    [Area("User")]
//    [Authorize]
//    public class BookingController : Controller
//    {
//        private readonly ApplicationDbContext _Context;
//        private readonly UserManager<ApplicationUser> _UserManager;
//        private readonly IWebHostEnvironment _environment;

//        public BookingController(ApplicationDbContext Context, UserManager<ApplicationUser> UserManager, IWebHostEnvironment environment)
//        {
//            _Context = Context;
//            _UserManager = UserManager;
//            _environment = environment;
//        }


//        // GET Add Booking
//        [HttpGet]
//        public async Task<IActionResult> AddBooking()
//        {
//            await LoadDropDownList();

//            return View(new Booking());
//        }



//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> AddBooking(Booking model, List<int> ServiceIds)
//        {
//            if (!ModelState.IsValid)
//            {
//                await LoadDropDownList();
//                return View(model);
//            }

//            model.Location = await _Context.Locations
//                .FirstOrDefaultAsync(l => l.Location_Id == model.Location_Id);

//            var user = await _UserManager.GetUserAsync(User);

//            var customer = await _Context.Customers
//                .FirstOrDefaultAsync(c => c.UserId == user.Id);

//            if (customer == null)
//            {
//                return NotFound("Customer not found");
//            }


//            model.Customer_Id = customer.Customer_Id;

//            var services = await _Context.Services
//                .Where(s => ServiceIds.Contains(s.Service_Id))
//                .ToListAsync();


//            decimal servicePrice = services.Sum(s => s.Price);


//            decimal locationPrice = await _Context.Locations
//                .Where(l => l.Location_Id == model.Location_Id)
//                .Select(l => l.Price)
//                .FirstOrDefaultAsync();


//            decimal subTotal =
//                model.BasePrice +
//                servicePrice +
//                locationPrice;


//            decimal serviceFee =
//                subTotal * Booking.Service_Fees;


//            decimal totalPrice =
//                subTotal + serviceFee;

//            if (model.ImageFile != null)
//            {
//                string fileName = Guid.NewGuid().ToString()
//                    + Path.GetExtension(model.ImageFile.FileName);


//                string folder = Path.Combine(
//                    _environment.WebRootPath,
//                    "images",
//                    "Booking"
//                );


//                if (!Directory.Exists(folder))
//                    Directory.CreateDirectory(folder);


//                string path = Path.Combine(folder, fileName);


//                using (var stream = new FileStream(path, FileMode.Create))
//                {
//                    await model.ImageFile.CopyToAsync(stream);
//                }


//                model.Note_Image = fileName;
//            }

//            var vm = new BookingConfirmViewModel
//            {
//                Booking = model,

//                Services = services,

//                ServiceIds = ServiceIds,

//                BasePrice = model.BasePrice,

//                ServicePrice = servicePrice,

//                LocationPrice = locationPrice,

//                ServiceFee = serviceFee,

//                TotalPrice = totalPrice,
//                Note_Image = model.Note_Image,
//                Customer = model.Customer_Id

//            };

//            return View("BookingConfirmViewModel", vm);
//        }




//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> ConfirmBooking(BookingConfirmViewModel vm)
//        {
//            var booking = vm.Booking;

//            if (booking == null)
//            {
//                return BadRequest();
//            }
//            var paymentMethod = booking.paymentMethod;

//            Console.WriteLine(paymentMethod);

//            booking.paymentMethod = paymentMethod;
//            booking.TotalPrice = vm.TotalPrice;
//            booking.Note_Image = vm.Note_Image;


//            booking.bookingStatus = Booking.BookingStatus.pending;



//            var userId = _UserManager.GetUserId(User);

//            var customer = await _Context.Customers


//      .FirstOrDefaultAsync(c => c.UserId == userId);


//            if (customer == null)
//            {
//                return BadRequest("Customer not found");
//            }


//            booking.Customer_Id = customer.Customer_Id;
//            booking.Customer = customer;

//            // Save Booking
//            _Context.Bookings.Add(booking);

//            await _Context.SaveChangesAsync();



//            // Save Services


//            foreach (var serviceId in vm.ServiceIds)
//            {
//                var bookingService = new BookingService
//                {
//                    Booking_Id = booking.Booking_Id,
//                    Service_Id = serviceId
//                };


//                _Context.BookingServices.Add(bookingService);
//            }


//            await _Context.SaveChangesAsync();


//            if (paymentMethod == PaymentMethodType.Cash)
//            {
//                var payment = new Payment
//                {
//                    Booking_Id = booking.Booking_Id,
//                    Amount = booking.TotalPrice,
//                    Currency = "JOD",
//                    Payment_Method = Payment.PaymentMethod.Cash,
//                    Status = Payment.PaymentStatus.Pending,
//                    Transaction_Id = "null"
//                };

//                _Context.Payments.Add(payment);
//                booking.bookingStatus = Booking.BookingStatus.pending;

//                await _Context.SaveChangesAsync();

//                return View("Success", booking);
//            }

//            Console.WriteLine(paymentMethod);
//            return RedirectToAction("VisaPayment", new { id = booking.Booking_Id });
//        }


//        [HttpGet]
//        public async Task<IActionResult> VisaPayment(int id)
//        {
//            var booking = await _Context.Bookings.FindAsync(id);

//            if (booking == null)
//                return NotFound();

//            return View(booking);
//        }


//        [HttpPost]
//        [ActionName("VisaPayment")]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> VisaPaymentPost(int id)
//        {
//            var booking = await _Context.Bookings.FindAsync(id);

//            if (booking == null)
//                return NotFound();

//            var payment = new Payment
//            {
//                Booking_Id = booking.Booking_Id,
//                Amount = booking.TotalPrice,
//                Currency = "JOD",
//                Payment_Method = Payment.PaymentMethod.Visa,
//                Status = PaymentStatus.Paid,
//                Transaction_Id = Guid.NewGuid().ToString() // رقم تجريبي
//            };



//            _Context.Payments.Add(payment);


//            // محاكاة نجاح الدفع
//            booking.bookingStatus = Booking.BookingStatus.accepted;

//            await _Context.SaveChangesAsync();

//            return View("Success", booking);
//        }



//        public IActionResult Success()
//        {
//            return View();
//        }


//        // View Booking List
//        [HttpGet]
//        public async Task<IActionResult> ViewBooking()
//        {

//            var booking = await _Context.Bookings

// .Include(b => b.Customer)
//     .ThenInclude(c => c.User)

// .Include(b => b.Location)
// .Include(b => b.Background)
// .Include(b => b.FrameColor)
// .Include(b => b.RoseType)
// .Include(b => b.RoseColor1)
// .Include(b => b.RoseColor2)

// .ToListAsync();


//            return View(booking);
//        }



        


//        [HttpPost]
//        public async Task<IActionResult> ChangeActiveStatus(int id, bool isActive)
//        {
//            var booking = await _Context.Bookings
//                .FirstOrDefaultAsync(b => b.Booking_Id == id);

//            if (booking == null)
//                return NotFound();

//            booking.Is_Active = isActive;

//            await _Context.SaveChangesAsync();

//            return RedirectToAction("Index", "Home", new { area = "" });
//        }




//        private async Task LoadDropDownList()
//        {
//            ViewBag.Department = new SelectList(
//                await _Context.Departments.ToListAsync(),
//                "Department_Id",
//                "Department_Name");

//            ViewBag.Gender = new SelectList(
//                await _Context.Genders.ToListAsync(),
//                "Gender_Id",
//                "Gender_Name");

//            ViewBag.Location = new SelectList(
//                await _Context.Locations.ToListAsync(),
//                "Location_Id",
//                "City");

//            ViewBag.Backgrounds = new SelectList(
//                await _Context.Backgrounds.ToListAsync(),
//                "Background_Id",
//                "Background_Color");

//            ViewBag.FrameColors = new SelectList(
//                await _Context.FrameColors.ToListAsync(),
//                "FrameColor_Id",
//                "FrameColor_Color");

//            ViewBag.RoseTypes = new SelectList(
//                await _Context.RoseTypes.ToListAsync(),
//                "RoseType_Id",
//                "RoseType_Name");

//            ViewBag.RoseColors = new SelectList(
//                await _Context.RoseColors.ToListAsync(),
//                "RoseColor_Id",
//                "RoseColor_Name");

//            ViewBag.Services = await _Context.Services
//                .Where(s => s.Is_Active)
//                .ToListAsync();
//        }

//    }

//}

