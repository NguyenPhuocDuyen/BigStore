using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BigStore.Models
{
    public class DiscountType
    {
        [Key] 
        public int Id { get; set; }

        [Required, MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        public virtual ICollection<DiscountCode>? DiscountCodes { get; set; }
    }
}
