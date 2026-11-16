using Ayloul.Data;
using Ayloul.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ayloul.Areas.Admin.Models
{
    public class EmployeeVm
    {

        [Key]
        [ValidateNever]
        public int Employee { get; set; }

        [ValidateNever]
        public string UserId { get; set; } = string.Empty;
        [ValidateNever]

        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; }

        [Range(0, double.MaxValue)]
        [Required(ErrorMessage = "please Enter Employee Salary")]
        public decimal Employee_Salary { get; set; }

        [Required(ErrorMessage = "please Enter Employee Email")]
        [EmailAddress]
        [StringLength(50, ErrorMessage = "please Enter Employee Email between 50 char")]
        public string Employee_Email { get; set; }

        [Required(ErrorMessage = "please Enter Employee Phone")]
        [RegularExpression(@"^\d{10,15}$", ErrorMessage = "Phone number must contain 10 to 15 digits ")]
        public string Mobile_Phone { get; set; }

        [Required(ErrorMessage = "please Enter the Employee Age ")]
        public DateTime Employee_Date { get; set; }

        public string RoleName { get; set; }

        [ValidateNever]
        [DefaultValue(true)]
        public bool Is_Active { get; set; } = true;
        [ValidateNever]
        public string? Employee_Img { get; set; }

        [ValidateNever]
        public string? Department { get; set; }

        [ValidateNever]
        public string? Gender { get; set; }

        [ValidateNever]
        public string? Location { get; set; }

    }
}
