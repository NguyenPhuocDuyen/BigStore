using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BigStore.BusinessObject
{
    public class News
    {
        [Key]
        public int Id { get; set; }

        public string? UserId { get; set; }

        [Required, MaxLength(255)]
        public string Title { get; set; } = string.Empty;

        [Required, Column(TypeName = "nvarchar(max)")]
        public string Content { get; set; } = string.Empty;

        public string? Thumbnail { get; set; } = string.Empty;

        public bool? IsPublish { get; set; } = false;

        public DateTime? CreateAt { get; set; } = DateTime.Now;
        public DateTime? UpdateAt { get; set; } = DateTime.Now;
        public bool? IsDelete { get; set; }

        public virtual User? User { get; set; }
    }
}
