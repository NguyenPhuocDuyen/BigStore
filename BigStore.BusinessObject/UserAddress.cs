using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BigStore.BusinessObject
{
    public class UserAddress
    {
        [Key]
        public int Id { get; set; }

        public string? UserId { get; set; } 

        [Required]
        [MaxLength(255)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [Phone, MaxLength(20)]
        public string Phone { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "nvarchar(max)")]
        public string Address { get; set; } = string.Empty;

        public bool? IsDefault { get; set; } = false;

        public DateTime? CreateAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdateAt { get; set; } = DateTime.UtcNow;
        public bool? IsDelete { get; set; }

        public virtual User? User { get; set; }
    }
}
