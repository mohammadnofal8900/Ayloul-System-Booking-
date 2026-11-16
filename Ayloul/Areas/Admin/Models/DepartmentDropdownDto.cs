using System.ComponentModel.DataAnnotations;

namespace Ayloul.Areas.Admin.Models
{
    public class DepartmentDropdownDto
    {
        public int DepartmentId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please select the Department Name.")]
        public string DepartmentName { get; set; }
    }
}
