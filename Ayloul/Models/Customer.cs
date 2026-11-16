using Ayloul.Data;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ayloul.Models
{
    public class Customer
    {
        [Key]
        public int Customer_Id { get; set; }

        [ValidateNever]
        public string UserId { get; set; } = string.Empty;
        [ValidateNever]

        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; }

        [StringLength(50, MinimumLength = 3, ErrorMessage = "Please Enter between 3 to 50 char .")]
        public string? Note { get; set; }

        [ValidateNever]
        [DefaultValue(true)]
        public bool Is_Active { get; set; } = true;

        public ICollection<Booking>? Bookings { get; set; }


    }
}
