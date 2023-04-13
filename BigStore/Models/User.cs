using BigStore.Binder;
using BigStore.Validation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BigStore.Models
{
    public class User : IdentityUser
    {
        [MaxLength(255)]
        [ModelBinder(typeof(MyCheckNameBinding))]
        public string? FullName { get; set; }

        [DataType(DataType.Date)]
        [BirthDay]
        public DateTime? DOB { get; set; } = DateTime.Now;

        [MaxLength(255)]
        public string? ImageUrl { get; set; }
        public bool? IsDelete { get; set; }
        public int? ShopId { get; set; }

        public virtual Shop? Shop { get; set; }

        public virtual ICollection<Cart>? Carts { get; set; }
        public virtual ICollection<LikeProduct>? LikeProducts { get; set; }
        public virtual ICollection<Notification>? Notifications { get; set; }
        public virtual ICollection<Order>? Orders { get; set; }
        public virtual ICollection<ProductReport>? ProductReports { get; set; }
        public virtual ICollection<Review>? Reviews { get; set; }
        public virtual ICollection<UserAddress>? UserAddresss { get; set; }
        public virtual ICollection<News>? Newss { get; set; }
    }
}
