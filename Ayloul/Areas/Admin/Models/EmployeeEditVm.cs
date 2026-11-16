using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Ayloul.Areas.Admin.Models
{
    public class EmployeeEditVm
    {
        [Key]

        public int EmployeeId { get; set; }

        public string UserId {  get; set; }=string.Empty;

        [StringLength(30, ErrorMessage = "please Enter Employee Email between 30 char")]
        [Required(ErrorMessage = "please Enter Employee Name")]
        public string Full_Name { get; set; }

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

        [DefaultValue(true)]
        public bool Is_Active { get; set; } = true;

        [Required(ErrorMessage = "please Enter the Role ")]
        public string RoleName { get; set; } = string.Empty;

        public string? Employee_Img { get; set; }

        [Display(Name ="Gender")]
        public int Gender_Id { get; set; }
        [Display(Name = "Department")]
        public int Department_Id { get; set; }
        [Display(Name = "Location")]
        public int Location_Id { get; set; }

        
    }
}
