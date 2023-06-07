using System.ComponentModel.DataAnnotations;

namespace BigStore.BusinessObject
{
    public class Cart
    {
        [Key]
        public int Id { get; set; }
        public string? UserId { get; set; }
        [Required]
        public int ProductId { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        public virtual Product? Product { get; set; }
        public virtual User? User { get; set; }
    }
}
