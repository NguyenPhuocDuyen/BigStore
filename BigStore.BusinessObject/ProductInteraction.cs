using System.ComponentModel.DataAnnotations;

namespace BigStore.BusinessObject
{
    public class ProductInteraction
    {
        [Key]
        public int Id { get; set; }

        public string? UserId { get; set; }

        public int ProductId { get; set; }

        public int ProductInteractionStatusId { get; set; }

        public virtual User? User { get; set; }
        public virtual Product? Product { get; set; }
        public virtual ProductInteractionStatus? Status { get; set; }
    }
}
