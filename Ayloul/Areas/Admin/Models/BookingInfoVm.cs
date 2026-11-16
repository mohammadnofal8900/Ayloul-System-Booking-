namespace Ayloul.Areas.Admin.Models
{
    public class BookingInfoVm
    {


        public int BookingId { get; set; }
        public string FullName { get; set; }

        public string Description { get; set; } = null!;
        public string? Note { get; set; }
        public bool IsActive { get; set; }

        public int? Background { get; set; }
        public int FrameColor { get; set; }
        public int LocationId { get; set; }
        public int RoseType { get; set; }
        
        public List<int>? ServiceId { get; set; }
        public string? Location { get; set; }

        public int RoseColor1_Id { get; set; }
        public string? RoseColor1 { get; set; }


        public int? RoseColor2_Id { get; set; }
        public string? RoseColor2 { get; set; }
        public int BookingNumber { get; set; }
        public DateTime DateTime { get; set; }
        public int BasePrice { get; set; }
        public string? EventDate { get; set; }
        public string Name { get; set; } = null!;
        public string? NoteImage { get; set; }
        public string PhoneNumber { get; set; } = null!;

        public decimal TotalPrice { get; set; }
        public int CustomerId { get; set; }

        public DateTime OrderDate { get; set; }

    }
}
