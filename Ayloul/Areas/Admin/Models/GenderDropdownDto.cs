using System.ComponentModel.DataAnnotations;

namespace Ayloul.Areas.Admin.Models
{
    public class GenderDropdownDto
    {
        public int GenderId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please select the Gender Name.")]
        public string GenderName { get; set; }
    }
}
