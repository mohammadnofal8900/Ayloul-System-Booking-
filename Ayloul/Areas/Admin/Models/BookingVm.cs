using System.Text.Json.Serialization;
using static Ayloul.Models.Booking;

namespace Ayloul.Areas.Admin.Models
{
    public class BookingVm
    {
        public int Booking_Id { get; set; }
        
        public string FullName { get; set; }
        public bool IsActive { get; set; }
        public string? Location { get; set; }
        public string PhoneNumber { get; set; } = null!;
        public int BookingNumber { get; set; }
        public DateTime DateTime { get; set; }
        public decimal TotalPrice { get; set; }
        public int CustomerId { get; set; }

        public DateTime OrderDate { get; set; }

        public PaymentMethodType? PaymentMethod { get; set; } = PaymentMethodType.Cash;

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public enum PaymentMethodType
        {
            Cash = 0,
            Visa = 1
        }
        public BookingStatus bookingStatus { get; set; } = BookingStatus.pending;

    }
}
