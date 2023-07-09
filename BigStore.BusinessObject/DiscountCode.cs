using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata.Ecma335;

namespace BigStore.BusinessObject
{
    public class DiscountCode : BaseEntity
    {
        public string DiscountTypeId { get; set; } = null!;

        public string Code { get; set; } = null!;

        [Column(TypeName = "decimal(8,0)")]
        [Range(1, 99999999, ErrorMessage = "Value must be between 1 and 99999999")]
        public decimal Value { get; set; }

        [Range(1, int.MaxValue)]
        public int MaxValueDiscount { get; set; }

        [Range(minimum: 1, int.MaxValue, ErrorMessage = "Remaining Usage Count must be geater 1")]
        public int RemainingUsageCount { get; set; }

        public DateTime StartDate { get; set; } = DateTime.UtcNow;

        public DateTime EndDate { get; set; } = DateTime.UtcNow.AddDays(7);

        [ForeignKey(nameof(DiscountTypeId))]
        public virtual DiscountType DiscountType { get; set; } = null!;

        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
