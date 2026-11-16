using System.ComponentModel.DataAnnotations;

namespace Ayloul.Models
{
    public class Location
    {
        [Key]
        public int Location_Id { get; set; }

        [Required(ErrorMessage = "please Enter Rose City .")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Please Enter between 3 to 20 char .")]
        public string City { get; set; }

        [Required(ErrorMessage = "please Enter Price .")]
        public int Price { get; set; }

        public ICollection<Booking> Bookings { get; set; }
    }
}
