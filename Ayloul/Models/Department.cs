using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Ayloul.Models
{
    public class Department
    {
        
        
            [Key]
            [ValidateNever]
            public int Department_Id { get; set; }
            [Required(ErrorMessage = "please Enter the Department Name .")]
            [StringLength(50, ErrorMessage = "please Enter Department Name between 50 char .")]
            public string Department_Name { get; set; }
            [ValidateNever]
            public ICollection<Employee> Employees { get; set; }
        
    }
}
