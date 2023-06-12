using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BigStore.BusinessObject
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int OrderStatusId { get; set; }

        public string? UserId { get; set; }
        [Required]
        public int DiscountCodeId { get; set; }

        [Required, Column(TypeName = "decimal(18,0)")]
        [Range(1, 999999999999999999)]
        public decimal TotalPrice { get; set; }

        [MaxLength(255)]
        public string FullName { get; set; } = string.Empty;

        [Phone, MaxLength(20)]
        public string Phone { get; set; } = string.Empty;

        [Column(TypeName = "nvarchar(max)")]
        public string Address { get; set; } = string.Empty;

        public DateTime? CreateAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdateAt { get; set; } = DateTime.UtcNow;

        public virtual OrderStatus? OrderStatus { get; set; }
        public virtual User? User { get; set; }
        public virtual DiscountCode? DiscountCode { get; set; }

        public virtual ICollection<OrderDetail>? OrderDetails { get; set; }
    }
}
