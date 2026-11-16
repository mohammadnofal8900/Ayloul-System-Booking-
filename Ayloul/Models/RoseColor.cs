using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ayloul.Models
{
    public class RoseColor
    {

        [Key]
        public int RoseColor_Id { get; set; }

        [Required(ErrorMessage = "please Enter Rose Color .")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Please Enter between 3 to 20 char .")]
        public string RoseColor_Name { get; set; }
        public string? img { get; set; } = null;

        [NotMapped]
        public IFormFile? ImageFile { get; set; }

        //public string? Image_Path { get; set; }
    }
}
