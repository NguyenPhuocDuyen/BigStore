using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BigStore.BusinessObject
{
    public class Notification
    {
        [Key]
        public int Id { get; set; }

        public string? UserId { get; set; }

        [Required, Column(TypeName = "nvarchar(max)")]
        public string Message { get; set; } = string.Empty;

        public bool? IsRead { get; set; }

        public DateTime? CreateAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpadteAt { get; set; } = DateTime.UtcNow;

        public virtual User? User { get; set; }
    }
}
