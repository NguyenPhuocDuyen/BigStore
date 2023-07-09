using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BigStore.BusinessObject
{
    public class ProductImage : BaseEntity
    {
        public string ProductId { get; set; } = null!;

        [StringLength(255)]
        public string ImageUrl { get; set; } = string.Empty;

        [ForeignKey(nameof(ProductId))]
        public virtual Product Product { get; set; } = null!;
    }
}
