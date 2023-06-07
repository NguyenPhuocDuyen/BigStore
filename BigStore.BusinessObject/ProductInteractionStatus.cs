using System.ComponentModel.DataAnnotations;

namespace BigStore.BusinessObject
{
    public class ProductInteractionStatus
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public virtual ICollection<ProductInteraction>? Interactions { get; set; }
    }
}
