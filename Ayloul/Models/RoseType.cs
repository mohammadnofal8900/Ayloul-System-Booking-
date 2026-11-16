using System.ComponentModel.DataAnnotations;

namespace Ayloul.Models
{
    public class RoseType
    {
        [Key]
        public int RoseType_Id { get; set; }

        [Required(ErrorMessage = "please Enter Rose Type .")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Please Enter between 3 to 20 char .")]
        public string RoseType_Name { get; set; }

        [Required(ErrorMessage = "please Enter Rose Type .")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Please Enter between 3 to 50 char .")]
        public string RoseType_Description { get; set; }
        public string? img { get; set; }

        public ICollection<Booking>? Bookings { get; set; }


    }
}
