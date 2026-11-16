using System.ComponentModel.DataAnnotations;

namespace Ayloul.Models
{
    public class Gender
    {
        [Key]
        public int Gender_Id { get; set; }

        [Required]
        public string Gender_Name { get; set; }

        public ICollection<Employee> Employees { get; set; }
    }
}
