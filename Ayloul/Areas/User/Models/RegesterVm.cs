using System.ComponentModel.DataAnnotations;

namespace Ayloul.Areas.User.Models
{
    public class RegesterVm
    {
        

        [Required(ErrorMessage ="Please Enter Your Name .")]
        [Display(Name ="Full Name")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Please Enter between 3 to 30 char .")]
        public string Full_Name { get; set; }

        [Required(ErrorMessage ="Please Enter Your Email .")]
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage ="This Email is Used .")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Please Enter between 3 to 50 char .")]
        public string Email {  get; set; }

        [Required(ErrorMessage = "Please Enter Your Phone .")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Phone number must contain numbers only.")]
        public string Phone_Number { get; set; }

        [Required(ErrorMessage = "Please Enter Password .")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Compare("Password")]
        [Required(ErrorMessage = "Please Enter Confirm Password .")]
        public string ConfirmPassword {  get; set; }

        public bool RememberMe {  get; set; }
    }
    public class RegesterResponseVm
    {
        public string Token { get; set; } = string.Empty;
    }

}
