using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BigStore.BusinessObject
{
    public class Order : BaseEntity
    {
        public string OrderStatusId { get; set; } = null!;

        public string UserId { get; set; } = null!;

        public string DiscountCodeId { get; set; } = null!;

        [Column(TypeName = "decimal(18,0)")]
        [Range(1, 999999999999999999)]
        public decimal TotalPrice { get; set; }

        public string FullName { get; set; } = null!;

        [Phone]
        public string Phone { get; set; } = null!;

        public string Address { get; set; } = null!;

        [ForeignKey(nameof(OrderStatusId))]
        public virtual OrderStatus OrderStatus { get; set; } = null!;

        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; } = null!;

        [ForeignKey(nameof(DiscountCodeId))]
        public virtual DiscountCode DiscountCode { get; set; } = null!;

        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}
