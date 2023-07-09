using BigStore.BusinessObject.Binder;
using BigStore.BusinessObject.Validation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BigStore.BusinessObject
{
    public class User : IdentityUser
    {
        [ModelBinder(typeof(MyCheckNameBinding))]
        public string FullName { get; set; } = null!;

        [DataType(DataType.Date)]
        [BirthDay(ErrorMessage = "Phải trên 16 tuổi")]
        public DateTime DOB { get; set; } = DateTime.UtcNow;

        public string ImageUrl { get; set; } = null!;

        public bool ProductSubscriber { get; set; } = false;

        public string? ShopId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; } = false;

        [ForeignKey(nameof(ShopId))]
        public virtual Shop? Shop { get; set; } 

        public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
        public virtual ICollection<UserAddress> UserAddresss { get; set; } = new List<UserAddress>();
    }
}
