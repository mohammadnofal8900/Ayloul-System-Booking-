using Ayloul.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ayloul.Models
{
    public class ChatRoom
    {
        [Key]
        public int ChatRoom_Id { get; set; }

        public string Customer_Id { get; set; }

        [ForeignKey(nameof(Customer_Id))]
        public ApplicationUser Customer { get; set; }

        public string Employee_Id { get; set; }

        [ForeignKey(nameof(Employee_Id))]
        public ApplicationUser Employee { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public ICollection<Message> Messages { get; set; } = new List<Message>();
    }
}
