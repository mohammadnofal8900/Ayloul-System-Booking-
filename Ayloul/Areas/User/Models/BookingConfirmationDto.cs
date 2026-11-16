namespace Ayloul.Areas.User.Models
{
    public class BookingConfirmationDto
    {
        public int CustomerId { get; set; }

        public string? Name { get; set; }
        public string? PhoneNumber { get; set; }

        public DateTime? DeliverDate { get; set; }
        public string? Description { get; set; }
        public string? Note { get; set; }
        public string? NoteImage { get; set; }

        public DateTime? EventDate { get; set; }

        public int LocationId { get; set; }
        public string? LocationName { get; set; }
        public decimal LocationPrice { get; set; }

        public int? BackgroundId { get; set; }
        public int? FrameColorId { get; set; }
        public int? RoseColor1Id { get; set; }
        public int? RoseColor2Id { get; set; }
        public int? RoseTypeId { get; set; }

        public List<int>? ServiceId { get; set; }

        public decimal BasePrice { get; set; }
        public decimal ServiceFees { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
