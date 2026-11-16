using Ayloul.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Ayloul.Data
{
    public class ApplicationUser :IdentityUser
    {
        [Required(ErrorMessage = "please Enter The Full Name !!")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Please Enter between 3 to 20 char !!")]
        public string FUllName { get; set; }

        [ValidateNever]
        [DefaultValue(true)]
        public bool Is_Active { get; set; } = true;

        [StringLength(50, MinimumLength = 3, ErrorMessage = "Please Enter between 3 to 50 char !!")]
        public string? Note { get; set; } = null;


        public ICollection<Customer> Customers { get; set; } = new List<Customer>();

    }
}
