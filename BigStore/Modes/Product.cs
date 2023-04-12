using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BigStore.Modes
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        public int ShopId { get; set; }

        [Required, MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        [Required, Column(TypeName = "nvarchar(max)")]
        public string Description { get; set; } = string.Empty;

        [Required, Column(TypeName = "decimal(10,0)")]
        [Range(1, 9999999999, ErrorMessage = "Price must be between 1 and 9999999999")]
        public decimal Price { get; set; } = 0;

        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; } = 0;

        public DateTime? CreateAt { get; set; } = DateTime.Now;
        public DateTime? UpdateAt { get; set; } = DateTime.Now;
        public bool? IsDelete { get; set; }

        public virtual Shop? Shop { get; set; }
        public virtual Category? Category { get; set; }

        public virtual ICollection<Cart>? Carts { get; set; }
        public virtual ICollection<LikeProduct>? LikeProducts { get; set; }
        public virtual ICollection<OrderDetail>? OrderDetails { get; set; }
        public virtual ICollection<ProductImage>? ProductImages { get; set; }
        public virtual ICollection<ProductReport>? ProductReports { get; set; }
        public virtual ICollection<Review>? Reviews { get; set; }
    }
}
