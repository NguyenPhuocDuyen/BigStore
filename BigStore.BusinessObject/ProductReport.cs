using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BigStore.BusinessObject
{
    public class ProductReport
    {
        [Key]
        public int Id { get; set; }

        public string? UserId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public int ReportStatusId { get; set; }

        [Required, Column(TypeName = "nvarchar(max)")]
        public string Reason { get; set; } = string.Empty;

        public DateTime? CreateAt { get; set; } = DateTime.Now;
        public DateTime? UpdateAt { get; set; } = DateTime.Now;

        public virtual User? User { get; set; }
        public virtual Product? Product { get; set; }
        public virtual ReportStatus? ReportStatus { get; set; }
    }
}
