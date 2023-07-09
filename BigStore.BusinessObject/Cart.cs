using System.ComponentModel.DataAnnotations.Schema;

namespace BigStore.BusinessObject
{
    public class Cart : BaseEntity
    {
        public string UserId { get; set; } = null!;

        public string ProductId { get; set; } = null!;

        public int Quantity { get; set; }

        [ForeignKey(nameof(ProductId))]
        public virtual Product Product { get; set; } = null!;

        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; } = null!;
    }
}
