using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace BigStore.BusinessObject
{
    public class ReportStatus
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        public virtual ICollection<ProductReport>? ProductReports { get; set; }
        public virtual ICollection<ReportProductReview>? ReportProductReviews { get; set; }
    }
}
