using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BigStore.Models
{
    public class Chat
    {
        [Key]
        public int Id { get; set; }

        public string SenderId { get; set; }
        [ForeignKey(nameof(SenderId))]
        public virtual User? Sender { get; set; }

        public string ReceiverId { get; set; }
        [ForeignKey(nameof(ReceiverId))]
        public virtual User? Receiver { get; set; }

        public required string Message { get; set; }

        public bool IsRead { get; set; } = false;

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
