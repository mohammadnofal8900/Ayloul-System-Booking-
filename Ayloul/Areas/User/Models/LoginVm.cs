using System.ComponentModel.DataAnnotations;

namespace Ayloul.Areas.User.Models
{
    public class LoginVm
    {
        


        [Required(ErrorMessage = "Please Enter Password .")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please Enter Your Email .")]
        [Display(Name = "Email")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Please Enter between 3 to 50 char .")]
        public string Email { get; set; }

        public bool RememberMe { get; set; }
    }
    public class LoginResponseVm
    {
        public string Token { get; set; } = string.Empty;
    }
}
