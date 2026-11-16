using Ayloul.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Ayloul.Areas.Admin.Models
{
    public class FinancialReportVM
    {
        public decimal TotalRevenue { get; set; }
        public decimal TodayRevenue { get; set; }
        public decimal MonthRevenue { get; set; }
        public int TotalBookings { get; set; }
        public int CompletedBookings { get; set; }

        public int PendingBookings { get; set; }

        public int AcceptedBookings { get; set; }


        public decimal ThisMonthRevenue { get; set; }
        public List<MonthlyProfitVM> MonthlyProfits { get; set; }
    = new List<MonthlyProfitVM>();

        public class MonthlyProfitVM
        {
            public string Month { get; set; }

            public decimal Profit { get; set; }
        }
        public List<Employee> Employees { get; set; }

    }
}
