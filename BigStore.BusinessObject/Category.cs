using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BigStore.BusinessObject
{
    public class Category : BaseEntity
    {
        // Category cha (FKey)
        [Display(Name = "Danh mục cha")]
        public string? ParentCategoryId { get; set; }

        [Required(ErrorMessage = "Phải có tên danh mục")]
        [StringLength(255, MinimumLength = 3, ErrorMessage = "{0} dài {1} đến {2}")]
        [Display(Name = "Tên danh mục")]
        public string Title { get; set; } = null!;
        
        [Display(Name = "Mô tả danh mục")]
        public string Description { get; set; } = null!;

        //chuỗi Url
        //[Required(ErrorMessage = "Phải tạo url")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "{0} dài {1} đến {2}")]
        [RegularExpression(@"^[a-z0-9-]*$", ErrorMessage = "Chỉ dùng các ký tự [a-z0-9-]")]
        [Display(Name = "Url hiện thị")]
        public string Slug { set; get; } = null!;

        [Display(Name = "Hình ảnh")]
        public string ImageUrl { get; set; } = null!;

        [ForeignKey(nameof(ParentCategoryId))]
        [Display(Name = "Danh mục cha")]
        public Category? ParentCategory { set; get; }

        public virtual ICollection<Product> Products { get; set; } = new List<Product>();

        // Các Category con
        public ICollection<Category> CategoryChildren { get; set; } = new List<Category>();
    }
}
