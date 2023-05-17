using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BigStore.Models
{
    public class Notification
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required, Column(TypeName = "nvarchar(max)")]
        public string Message { get; set; } = string.Empty;

        public bool? IsRead { get; set; }

        public DateTime? CreateAt { get; set; } = DateTime.Now;
        public DateTime? UpadteAt { get; set; } = DateTime.Now;

        public virtual User? User { get; set; }
    }
}
