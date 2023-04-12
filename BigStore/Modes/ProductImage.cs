using System.ComponentModel.DataAnnotations;

namespace BigStore.Modes
{
    public class ProductImage
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required, StringLength(255)]
        public string ImageUrl { get; set; } = string.Empty;

        public DateTime? CreatAt { get; set; } = DateTime.Now;
        public bool? IsDelete { get; set; }

        public virtual Product? Product { get; set; }
    }
}
