using System.ComponentModel.DataAnnotations;

namespace Ayloul.Areas.Admin.Models
{
    public class RoseColorDropdownDto
    {
        public int RoseColor_Id { get; set; }

        [Required(ErrorMessage = "please Enter Rose Color .")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Please Enter between 3 to 20 char .")]
        public string RoseColor_Name { get; set; }

        public string? img { get; set; } = null;
    }
}
