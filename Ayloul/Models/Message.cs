using Ayloul.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ayloul.Models
{
    public class Message
    {
        [Key]
        public int Message_Id { get; set; }

        public int ChatRoom_Id { get; set; }

        [ForeignKey(nameof(ChatRoom_Id))]
        public ChatRoom ChatRoom { get; set; }

        public string Sender_Id { get; set; }

        [ForeignKey(nameof(Sender_Id))]
        public ApplicationUser Sender { get; set; }

        [Required]
        public string Message_Text { get; set; }

        public DateTime SentAt { get; set; } = DateTime.Now;

        public bool IsRead { get; set; } = false;
    }
}
