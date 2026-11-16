using System.ComponentModel.DataAnnotations;

namespace Ayloul.Areas.Admin.Models
{
    public class BackgroundDropdownDto
    {

        public int BackgroundId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please select the Background color.")]
        public string BackgroundColor { get; set; } = null!;

        public string? img { get; set; } = null;
    }
}
