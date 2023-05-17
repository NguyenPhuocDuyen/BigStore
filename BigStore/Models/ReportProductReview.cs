using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BigStore.Models
{
    public class ReportProductReview
    {
        [Key] 
        public int Id { get; set; }
        public string? UserId { get; set; }
        public int ProductReviewId { get; set; }
        public int ReportStatusId { get; set; }

        [Required, Column(TypeName = "nvarchar(max)")]
        public string Reason { get; set; } = string.Empty;

        public DateTime? CreateAt { get; set; } = DateTime.Now;
        public DateTime? UpdateAt { get; set; } = DateTime.Now;

        public virtual User? User { get; set; }
        public virtual ProductReview? ProductReview { get; set; }
        public virtual ReportStatus? ReportStatus { get; set; }
    }
}
