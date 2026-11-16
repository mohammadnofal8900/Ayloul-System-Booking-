using Ayloul.Data;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ayloul.Models
{
    public class Employee
    {

        [Key]
        [ValidateNever]
        public int Employee_Id { get; set; }

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
        [ValidateNever]
        [DefaultValue(true)]
        public bool Is_Active { get; set; } = true;
        [ValidateNever]
        public string? Employee_Img { get; set; }
        [ValidateNever]
        public int Gender_Id { get; set; }
        [ValidateNever]
        public int Department_Id { get; set; }
        [ValidateNever]
        [ForeignKey("Department_Id")]
        public Department department { get; set; }

        [ValidateNever]
        [ForeignKey("Gender_Id")]
        public Gender Gender { get; set; }

        [ValidateNever]
        public int Location_Id { get; set; }

        [ValidateNever]
        [ForeignKey("Location_Id")]
        public Location Location { get; set; }


    }
}
