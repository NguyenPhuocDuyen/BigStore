using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BigStore.BusinessObject
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Danh mục sản phẩm")]
        public int? CategoryId { get; set; }

        [Required]
        public int ShopId { get; set; }

        [Required, MaxLength(255)]
        [Display(Name = "Tên sản phẩm")]
        public string Name { get; set; } = string.Empty;

        [Required, Column(TypeName = "nvarchar(max)")]
        [Display(Name = "Mô tả sản phẩm")]
        public string Description { get; set; } = string.Empty;

        [Required, Column(TypeName = "decimal(10,0)")]
        [Display(Name = "Giá sản phẩm")]
        [Range(1, 9999999999, ErrorMessage = "Price must be between 1 and 9999999999")]
        public decimal Price { get; set; } = 0;

        [Required]
        [Range(1, int.MaxValue)]
        [Display(Name = "Số lượng sản phẩm")]
        public int Quantity { get; set; } = 0;

        //chuỗi Url
        [Required(ErrorMessage = "Phải tạo url")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "{0} dài {1} đến {2}")]
        [RegularExpression(@"^[a-z0-9-]*$", ErrorMessage = "Chỉ dùng các ký tự [a-z0-9-]")]
        [Display(Name = "Url hiện thị")]
        public string Slug { set; get; } = string.Empty;

        public DateTime? CreateAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdateAt { get; set; } = DateTime.UtcNow;
        public bool? IsDelete { get; set; } = false;

        public virtual Shop? Shop { get; set; }
        public virtual Category? Category { get; set; }

        public virtual ICollection<Cart>? Carts { get; set; }
        public virtual ICollection<OrderDetail>? OrderDetails { get; set; }
        public virtual ICollection<ProductImage>? ProductImages { get; set; }
        public virtual ICollection<ProductReport>? ProductReports { get; set; }
        public virtual ICollection<ProductReview>? Reviews { get; set; }
        public virtual ICollection<FavoriteProduct>? FavoriteProducts { get; set; }
        public virtual ICollection<ProductInteraction>? Interactions { get; set; }
    }
}
