using System.ComponentModel.DataAnnotations;

namespace BigStore.Models
{
    public class FavoriteProduct
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? UserId { get; set; }

        [Required]
        public int ProductId { get; set; }

        public virtual User? User { get; set; }
        public virtual Product? Product { get; set; }

    }
}
