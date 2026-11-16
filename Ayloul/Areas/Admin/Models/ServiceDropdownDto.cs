using System.ComponentModel.DataAnnotations;

namespace Ayloul.Areas.Admin.Models
{
    public class ServiceDropdownDto
    {
        public int Service_Id { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please select the Service Name .")]
        public string Service_Name { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please select the Price .")]
        public int Price { get; set; }

        
        public string? img { get; set; }
    }
}
