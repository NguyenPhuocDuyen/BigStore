using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BigStore.Models
{
    public class Category
    {
        [Key] 
        public int Id { get; set; }

        [Required, MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        [Required, Column(TypeName = "nvarchar(max)")]
        public string Description { get; set; } = string.Empty;

        [MaxLength(255)]
        public string? ImageUrl { get; set; } = string.Empty;
        
        public DateTime? CreateAt { get; set; } = DateTime.Now;
        
        public DateTime? UpdateAt { get; set; } = DateTime.Now;

        public bool? IsDelete { get; set; }

        public virtual ICollection<Product>? Products { get; set; }
    }
}
