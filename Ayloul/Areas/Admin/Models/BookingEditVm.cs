using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Ayloul.Models.Booking;

namespace Ayloul.Areas.Admin.Models
{
    public class BookingEditVm
    {


        public int BookingId { get; set; }

        [Display(Name = "Name")]
        [Required(ErrorMessage = "please Enter Name .")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Please Enter between 3 to 20 char .")]
        public string Name { get; set; }
        public int CustomerId { get; set; }


        [Required(ErrorMessage = "please Enter Description .")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Please Enter between 3 to 50 char .")]
        public string Description { get; set; } = null!;

        [Display(Name = "Special date or event date")]
        public string? EventDate { get; set; }

        [Required(ErrorMessage = "Please select the Location .")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select the Location .")]
        public int LocationId { get; set; }
        [Required(ErrorMessage = "please Enter the Booking Status .")]
        public BookingStatus bookingStatus { get; set; } = BookingStatus.pending;

        [ValidateNever]
        public List<int>? ServiceId { get; set; } = new List<int>();

        [Required(ErrorMessage = "Please select the Background color.")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select the Background color.")]
        public int? Background { get; set; }

        [Required(ErrorMessage = "Please select the Frame rose color")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select the Frame rose color.")]
        public int FrameColor { get; set; }

        [Required(ErrorMessage = "Please select the first rose color .")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select the first rose color.")]
        public int RoseColor1_Id { get; set; }
        public int? RoseColor2_Id { get; set; }

        [Required(ErrorMessage = "Please select the rose Type. ")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select the rose Type.")]
        public int RoseType { get; set; }

        [StringLength(500, MinimumLength = 3, ErrorMessage = "Please Enter between 3 to 500 char .")]
        public string? Note { get; set; }
        public bool IsActive { get; set; }


        public string? NoteImage { get; set; }


        [NotMapped]
        [Display(Name = "Note Image")]
        public IFormFile? ImageFile { get; set; }

    }
}
