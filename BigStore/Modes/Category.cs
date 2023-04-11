using System.ComponentModel.DataAnnotations;

namespace BigStore.Modes
{
    public class Category
    {
        [Key] 
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        public string Description { get; set; } = string.Empty;

        [MaxLength(255)]
        public string? ImageUrl { get; set; } = string.Empty;
        
        public DateTime? CreateAt { get; set; } = DateTime.Now;
        
        public DateTime? UpdateAt { get; set; } = DateTime.Now;

        public bool IsDelete { get; set; } = false;
    }
}
