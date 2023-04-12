using System.ComponentModel.DataAnnotations;

namespace BigStore.Modes
{
    public class LikeProduct
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int ProductId { get; set; }

        public virtual User? User { get; set; }
        public virtual Product? Product { get; set; }
    }
}
