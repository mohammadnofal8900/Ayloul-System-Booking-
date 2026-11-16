using System.ComponentModel.DataAnnotations;

namespace Ayloul.Areas.User.Models
{
    public class EditProfileViewModel
    {
        public string Id { get; set; }

        public string? FUllName { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Note { get; set; }

        public string? Email { get; set; }

        [DataType(DataType.Password)]
        public string? CurrentPassword { get; set; }


        [DataType(DataType.Password)]
        public string? NewPassword { get; set; }


        [DataType(DataType.Password)]
        [Compare(
            "NewPassword",
            ErrorMessage = "Passwords do not match."
        )]
        public string? ConfirmPassword { get; set; }
    }
}
