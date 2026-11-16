using System.ComponentModel.DataAnnotations;

namespace Ayloul.Areas.Admin.Models
{
    public class FrameColorDropdownDto
    {

        public int FrameColor_Id { get; set; }

        public string? img { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please select the Frame color.")]
        public string FrameColor_Color { get; set; }


    }
}
