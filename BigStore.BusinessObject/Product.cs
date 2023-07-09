using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BigStore.BusinessObject
{
    public class Product : BaseEntity
    {
        [Display(Name = "Danh mục sản phẩm")]
        public string? CategoryId { get; set; }

        public string ShopId { get; set; } = null!;

        [Display(Name = "Tên sản phẩm")]
        public string Name { get; set; } = null!;

        [Display(Name = "Mô tả sản phẩm")]
        public string Description { get; set; } = null!;

        [Column(TypeName = "decimal(10,0)")]
        [Display(Name = "Giá sản phẩm")]
        [Range(1, 9999999999, ErrorMessage = "Price must be between 1 and 9999999999")]
        public decimal Price { get; set; } = 0;

        [Range(1, int.MaxValue)]
        [Display(Name = "Số lượng sản phẩm")]
        public int Quantity { get; set; } = 0;

        //chuỗi Url
        //[Required(ErrorMessage = "Phải tạo url")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "{0} dài {1} đến {2}")]
        [RegularExpression(@"^[a-z0-9-]*$", ErrorMessage = "Chỉ dùng các ký tự [a-z0-9-]")]
        [Display(Name = "Url hiện thị")]
        public string Slug { set; get; } = null!;

        [ForeignKey(nameof(ShopId))]
        public virtual Shop Shop { get; set; } = null!;

        [ForeignKey(nameof(CategoryId))]
        public virtual Category Category { get; set; } = null!;

        public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();
        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
        public virtual ICollection<ProductImage> ProductImages { get; set; } = new List<ProductImage>();
    }
}
