using Ayloul.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ayloul.Models
{
    public class Rating
    {
        [Key]
        public int Rating_Id { get; set; }

        public int Booking_Id { get; set; }

        [ForeignKey(nameof(Booking_Id))]
        public Booking Booking { get; set; }

        public int Customer_Id { get; set; }

        [ForeignKey(nameof(Customer_Id))]
        public Customer Customer { get; set; }

        public int Employee_Id { get; set; }

        [ForeignKey(nameof(Employee_Id))]
        public Employee Employee { get; set; }

        [Range(1, 5)]
        public int Stars { get; set; }

        [StringLength(500)]
        public string? Comment { get; set; }

        public DateTime Created_At { get; set; } = DateTime.Now;
    }

}
