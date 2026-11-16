using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ayloul.Models
{
    public class BookingService
    {
        [Key]
        public int BookingService_Id { get; set; }
        public int Booking_Id { get; set; }
        [ForeignKey(nameof(Booking_Id))]
        public Booking Booking { get; set; }

        public int Service_Id { get; set; }
        [ForeignKey(nameof(Service_Id))]
        public Service Service { get; set; }
    }
}
