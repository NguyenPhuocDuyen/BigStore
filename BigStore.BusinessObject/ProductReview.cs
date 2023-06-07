using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BigStore.BusinessObject
{
    public class ProductReview
    {
        [Key]
        public int Id { get; set; }

        public string? UserId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public int Rate { get; set; }

        [Required, Column(TypeName = "nvarchar(max)")]
        public string Description { get; set; } = string.Empty;

        public DateTime? CreateAt { get; set; } = DateTime.Now;
        public DateTime? UpdateAt { get; set; } = DateTime.Now;

        public bool? IsDelete { get; set; }

        public virtual User? User { get; set; }
        public virtual Product? Product { get; set; }
    }
}
