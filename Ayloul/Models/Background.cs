using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ayloul.Models
{
    public class Background
    {
        [Key]
        public int Background_Id { get; set; }

        [Required(ErrorMessage = "please Enter Background Color .")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Please Enter between 3 to 20 char .")]
        public string Background_Color { get; set; }
        [ValidateNever]
        public string? img { get; set; }


        [NotMapped]
        public IFormFile? ImageFile { get; set; }

        //public string? Image_Path { get; set; }
        
        [ValidateNever]
        public ICollection<Booking> Bookings { get; set; }

    }
}
