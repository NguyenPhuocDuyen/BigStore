using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BigStore.BusinessObject
{
    public class Shop : BaseEntity
    {
        //[ScaffoldColumn(false)]
        public string UserId { get; set; } = null!;

        public string ShopName { get; set; } = null!;

        public string Description { get; set; } = null!;

        [Phone]
        public string Phone { get; set; } = null!;

        [Column(TypeName = "nvarchar(max)")]
        public string Address { get; set; } = null!;

        public string ImageUrl { get; set; } = null!;

        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; } = null!;

        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
