using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BigStore.BusinessObject
{
    public class Shop
    {
        [Key]
        public int Id { get; set; }

        //[Required]
        [ScaffoldColumn(false)]
        public string? UserId { get; set; } = string.Empty;

        [Required, MaxLength(255)]
        public string ShopName { get; set; } = string.Empty;

        [Required, Column(TypeName = "nvarchar(max)")]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Phone, MaxLength(20)]
        public string Phone { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "nvarchar(max)")]
        public string Address { get; set; } = string.Empty;

        [MaxLength(255)]
        public string? ImageUrl { get; set; } = string.Empty;

        public DateTime? CreateAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdateAt { get; set; } = DateTime.UtcNow;
        
        public virtual User? User { get; set; }

        public virtual ICollection<Product>? Products { get; set; }
    }
}
