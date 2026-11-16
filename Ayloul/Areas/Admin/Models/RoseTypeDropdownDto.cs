using System.ComponentModel.DataAnnotations;

namespace Ayloul.Areas.Admin.Models
{
    public class RoseTypeDropdownDto
    {
        public int RoseType_Id { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please select the Rose Type.")]
        public string RoseType_Name { get; set; }
        public string RoseType_Description { get; set; }
        public string? img { get; set; }
    }
}
