using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BigStore.BusinessObject
{
    public class Category
    {
        [Key] 
        public int Id { get; set; }

        // Category cha (FKey)
        [Display(Name = "Danh mục cha")]
        public int? ParentCategoryId { get; set; }

        [Required(ErrorMessage = "Phải có tên danh mục")]
        [StringLength(255, MinimumLength = 3, ErrorMessage = "{0} dài {1} đến {2}")]
        [Display(Name = "Tên danh mục")]
        public string Title { get; set; } = string.Empty;

        [Required, Column(TypeName = "nvarchar(max)")]
        [Display(Name = "Mô tả danh mục")]
        public string Description { get; set; } = string.Empty;

        //chuỗi Url
        //[Required(ErrorMessage = "Phải tạo url")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "{0} dài {1} đến {2}")]
        [RegularExpression(@"^[a-z0-9-]*$", ErrorMessage = "Chỉ dùng các ký tự [a-z0-9-]")]
        [Display(Name = "Url hiện thị")]
        public string? Slug { set; get; }

        [MaxLength(255)]
        [Display(Name = "Hình ảnh")]
        public string? ImageUrl { get; set; } = string.Empty;

        [Display(Name = "Ngày tạo")]
        public DateTime? CreateAt { get; set; } = DateTime.UtcNow;

        [Display(Name = "Ngày cập nhật")] 
        public DateTime? UpdateAt { get; set; } = DateTime.UtcNow;

        public bool? IsDelete { get; set; } = false;

        [ForeignKey("ParentCategoryId")]
        [Display(Name = "Danh mục cha")]
        public Category? ParentCategory { set; get; }

        public virtual ICollection<Product>? Products { get; set; }

        // Các Category con
        public ICollection<Category>? CategoryChildren { get; set; }

        
    }
}
