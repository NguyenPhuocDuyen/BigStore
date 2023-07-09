using Bogus.DataSets;
using System.ComponentModel.DataAnnotations;

namespace BigStore.Dtos.CategoryDto
{
    public class CategoryUpdateDto
    {
        public string Id { get; set; } = null!;

        // Category cha (FKey)
        [Display(Name = "Danh mục cha")]
        public string? ParentCategoryId { get; set; }

        [Required(ErrorMessage = "{0} là bắt buộc")]
        [StringLength(255, MinimumLength = 3, ErrorMessage = "{0} dài {1} đến {2}")]
        [Display(Name = "Tên danh mục")]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = "{0} là bắt buộc")]
        [Display(Name = "Mô tả danh mục")]
        public string Description { get; set; } = null!;

        public string? ImageUrl { get; set; }
    }
}
