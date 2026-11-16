using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ayloul.Areas.User.Models
{
    public class UserBookingEditVm
    {

        public int BookingId { get; set; }

        public string Name { get; set; } = null!;

        public string Phone_Number { get; set; } = null!;

        public DateTime Order_Date { get; set; }

        public string Description { get; set; } = null!;

        public string? Note { get; set; }

        public string? Note_Image { get; set; }

        public string? Event_Date { get; set; }

        public int Location_Id { get; set; }

        public List<int>? Service_Id { get; set; }

        public int? Background_Id { get; set; }

        public int FrameColor_Id { get; set; }

        public int RoseColor1_Id { get; set; }

        public int? RoseColor2_Id { get; set; }

        public int RoseType_Id { get; set; }

        
        [NotMapped]
        [Display(Name = "Note Image")]
        public IFormFile? ImageFile { get; set; }

    }
}
