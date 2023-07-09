using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BigStore.BusinessObject
{
    public class UserAddress : BaseEntity
    {
        public string UserId { get; set; } = null!;

        public string FullName { get; set; } = null!;

        public string Phone { get; set; } = null!;

        public string Address { get; set; } = null!;

        public bool IsDefault { get; set; } = false;

        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; } = null!;
    }
}
