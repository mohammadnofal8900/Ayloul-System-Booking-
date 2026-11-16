using Ayloul.Areas.Admin.Models;
using Ayloul.Data;
using Ayloul.Models;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using System.Threading.Tasks;
using static Ayloul.Areas.Admin.Models.FinancialReportVM;
using static Ayloul.Models.Booking;

namespace Ayloul.Areas.Admin.Controllers
{
    [Authorize(Roles = "Super Admin,Admin")]
    [Area("Admin")]
    [Authorize]
    public class ReportsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ReportsController(ApplicationDbContext Context, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _context = Context;
        }
        public async Task<IActionResult> FinancialReport()
        {
            var report = new FinancialReportVM();

            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);

            var monthStart = new DateTime(
                today.Year,
                today.Month,
                1);

            var nextMonth = monthStart.AddMonths(1);



            // ==============================
            // Today's Revenue
            // Completed bookings today only
            // ==============================


            report.TodayRevenue = await _context.Bookings
    .Where(b =>
        b.bookingStatus == BookingStatus.completed &&
        b.Order_Date >= today &&
        b.Order_Date < tomorrow)
    .SumAsync(b => (decimal?)b.TotalPrice) ?? 0;

            // ==============================
            // This Month Revenue
            // Completed bookings this month only
            // ==============================

            report.MonthRevenue = await _context.Bookings
    .Where(b =>
        b.bookingStatus == BookingStatus.completed &&
        b.Order_Date >= monthStart &&
        b.Order_Date < nextMonth)
    .SumAsync(b => (decimal?)b.TotalPrice) ?? 0;


            // ==============================
            // Total Revenue
            // All completed bookings
            // ==============================

            report.TotalRevenue = await _context.Bookings
     .Where(b =>
         b.bookingStatus == BookingStatus.completed)
     .SumAsync(b => (decimal?)b.TotalPrice) ?? 0;


            // ==============================
            // Booking Counts
            // ==============================

            report.CompletedBookings = await _context.Bookings
                .CountAsync(b =>
                    b.bookingStatus == BookingStatus.completed);

            report.AcceptedBookings = await _context.Bookings
                .CountAsync(b =>
                    b.bookingStatus == BookingStatus.accepted);

            report.PendingBookings = await _context.Bookings
                .CountAsync(b =>
                    b.bookingStatus == BookingStatus.pending);


            // ==============================
            // Monthly Revenue
            // ==============================

            var monthlyData = await _context.Bookings
                .Where(b =>
                    b.bookingStatus == BookingStatus.completed)
                .GroupBy(b => new
                {
                    Year = b.Date_Time.Year,
                    Month = b.Date_Time.Month
                })
                .Select(g => new
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    Profit = g.Sum(b => b.TotalPrice)
                })
                .OrderBy(x => x.Year)
                .ThenBy(x => x.Month)
                .ToListAsync();


            report.MonthlyProfits = monthlyData
                .Select(x => new MonthlyProfitVM
                {
                    Month = new DateTime(
                        x.Year,
                        x.Month,
                        1).ToString("MMM"),

                    Profit = x.Profit
                })
                .ToList();

            report.Employees = await _context.Employees
     .Include(d => d.department)
     .Include(u => u.User)
     .Include(g => g.Gender)
     .Include(l => l.Location)
     .ToListAsync();

            var roles = new Dictionary<string, string>();

            foreach (var employee in report.Employees)
            {
                var userRoles = await _userManager.GetRolesAsync(employee.User);

                roles.Add(
                    employee.UserId,
                    userRoles.FirstOrDefault() ?? "No Role"
                );
            }

            ViewBag.Roles = roles;
            return View(report);
        }





        public async Task<IActionResult> ExportExcel()
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Financial Report");

            worksheet.Cell(1, 1).Value = "Booking ID";
            worksheet.Cell(1, 2).Value = "Customer";
            worksheet.Cell(1, 3).Value = "Date";
            worksheet.Cell(1, 4).Value = "Location";
            worksheet.Cell(1, 5).Value = "Amount";

            var bookings = await _context.Bookings
       .Include(b => b.Customer)
       .ThenInclude(c => c.User)
       .Include(c=>c.Location)
       .ToListAsync();


            int row = 2;

            foreach (var item in bookings)
            {
                worksheet.Cell(row, 1).Value = item.Booking_Id;

                worksheet.Cell(row, 2).Value =
                    item.Customer?.User?.FUllName ?? "Unknown";

                worksheet.Cell(row, 3).Value =
                    item.Order_Date;
                worksheet.Cell(row, 4).Value =
                    item.Location?.City;

                worksheet.Cell(row, 5).Value =
                    item.TotalPrice;

                row++;
            }


            worksheet.Columns().AdjustToContents();


            using var stream = new MemoryStream();

            workbook.SaveAs(stream);

            return File(
                stream.ToArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "FinancialReport.xlsx");
        }


        public async Task<IActionResult> ExportPdf()
        {
            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);

            var monthStart = new DateTime(
                today.Year,
                today.Month,
                1);

            var nextMonth = monthStart.AddMonths(1);


            // =====================================
            // Total Revenue
            // =====================================

            var totalRevenue = await _context.Bookings
                .Where(b =>
                    b.bookingStatus == BookingStatus.completed)
                .SumAsync(b => (decimal?)b.TotalPrice) ?? 0;


            // =====================================
            // Today's Revenue
            // =====================================

            var todayRevenue = await _context.Bookings
                .Where(b =>
                    b.bookingStatus == BookingStatus.completed &&
                    b.Order_Date >= today &&
                    b.Order_Date < tomorrow)
                .SumAsync(b => (decimal?)b.TotalPrice) ?? 0;


            // =====================================
            // This Month Revenue
            // =====================================

            var monthRevenue = await _context.Bookings
                .Where(b =>
                    b.bookingStatus == BookingStatus.completed &&
                    b.Order_Date >= monthStart &&
                    b.Order_Date < nextMonth)
                .SumAsync(b => (decimal?)b.TotalPrice) ?? 0;


            // =====================================
            // Create PDF
            // =====================================

            var pdf = QuestPDF.Fluent.Document.Create(document =>
            {
                document.Page(page =>
                {
                    page.Margin(30);


                    // Header
                    page.Header()
                        .Text("Financial Report")
                        .FontSize(20)
                        .Bold();


                    // Content
                    page.Content()
                        .Column(column =>
                        {
                            column.Spacing(10);


                            column.Item()
                                .Text($"Today's Revenue: {todayRevenue:N2} JD")
                                .FontSize(14);


                            column.Item()
                                .Text($"This Month Revenue: {monthRevenue:N2} JD")
                                .FontSize(14);


                            column.Item()
                                .Text($"Total Revenue: {totalRevenue:N2} JD")
                                .FontSize(14);


                            column.Item()
                                .PaddingTop(15)
                                .Text($"Report Date: {DateTime.Now:yyyy-MM-dd HH:mm}")
                                .FontSize(11);
                        });


                    // Footer
                    page.Footer()
                        .AlignCenter()
                        .Text("Ayloul System");
                });
            })
            .GeneratePdf();


            return File(
                pdf,
                "application/pdf",
                "FinancialReport.pdf"
            );
        }



    }
}
