using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ayloul.Areas.Admin.Models
{
    public class ServicesApiVm
    {

        [Key]
        public int Service_Id { get; set; }

        [Required(ErrorMessage = "please Enter Service Name .")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Please Enter between 3 to 20 char .")]
        public string Service_Name { get; set; }

        [Required(ErrorMessage = "please Enter Service Price .")]
        public int Price { get; set; }

        [ValidateNever]
        [DefaultValue(true)]
        public bool Is_Active { get; set; } = true;

        public string? img { get; set; } = null;


        [NotMapped]
        public IFormFile? ImageFile { get; set; }
    }
}
