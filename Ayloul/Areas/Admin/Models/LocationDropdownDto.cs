using System.ComponentModel.DataAnnotations;

namespace Ayloul.Areas.Admin.Models
{
    public class LocationDropdownDto
    {

        public int LocationId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please select the City .")]
        public string City { get; set; }
    }
}
