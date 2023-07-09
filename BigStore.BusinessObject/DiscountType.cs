using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BigStore.BusinessObject
{
    public class DiscountType : BaseEntity
    {
        public string Name { get; set; } = null!;

        public virtual ICollection<DiscountCode> DiscountCodes { get; set; } = new List<DiscountCode>();
    }
}
