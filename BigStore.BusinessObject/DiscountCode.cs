using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata.Ecma335;

namespace BigStore.BusinessObject
{
    public class DiscountCode
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int DiscountTypeId { get; set; }

        [Required, StringLength(255)]
        public string Code { get; set; } = string.Empty;

        [Required, Column(TypeName = "decimal(8,0)")]
        [Range(1, 99999999, ErrorMessage = "Value must be between 1 and 99999999")]
        public decimal Value { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int MaxValueDiscount { get; set; }

        [Required]
        [Range(minimum: 1, int.MaxValue, ErrorMessage = "Remaining Usage Count must be geater 1")]
        public int RemainingUsageCount { get; set; }

        [Required]
        public DateTime StartDate { get; set; } = DateTime.Now;

        [Required]
        public DateTime EndDate { get; set; } = DateTime.Now;

        public DateTime? CreateAt { get; set; } = DateTime.Now;
        public DateTime? UpdateAt { get; set; } = DateTime.Now;

        public bool? IsDelete { get; set; }

        public virtual DiscountType? DiscountType { get; set; }

        public virtual ICollection<Order>? Orders { get; set; }
    }
}
