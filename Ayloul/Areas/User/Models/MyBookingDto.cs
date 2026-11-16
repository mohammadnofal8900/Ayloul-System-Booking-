namespace Ayloul.Areas.User.Models
{
    public class MyBookingDto
    {
        public int BookingId { get; set; }
        public int BookingNumber { get; set; }

        public string Name { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string? Note { get; set; }
        public string? NoteImage { get; set; }
        public string? EventDate { get; set; }

        public DateTime DeliveryDate { get; set; }
        public DateTime OrderDate { get; set; }

        public decimal TotalPrice { get; set; }

        public int BookingStatus { get; set; }
        public int? PaymentMethod { get; set; }

        public string? LocationName { get; set; }

        public string? BackgroundName { get; set; }
        public string? FrameColorName { get; set; }

        public string? RoseTypeName { get; set; }
        public string? RoseColor1Name { get; set; }
        public string? RoseColor2Name { get; set; }

        public List<string> Services { get; set; } = new();
    }
}
