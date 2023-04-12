using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BigStore.Modes
{
    public class ProductReport
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required, Column(TypeName = "nvarchar(max)")]
        public string Reason { get; set; } = string.Empty;

        public DateTime? CreateAt { get; set; }

        public virtual User? User { get; set; }
        public virtual Product? Product { get; set; }
    }
}
