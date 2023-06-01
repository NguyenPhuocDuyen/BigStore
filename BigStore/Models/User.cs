using BigStore.Binder;
using BigStore.Validation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BigStore.Models
{
    public class User : IdentityUser
    {
        [MaxLength(255)]
        [ModelBinder(typeof(MyCheckNameBinding))]
        public string? FullName { get; set; }

        [DataType(DataType.Date)]
        [BirthDay]
        public DateTime? DOB { get; set; } = DateTime.UtcNow;

        [MaxLength(255)]
        public string? ImageUrl { get; set; } = string.Empty;
        public bool? ProductSubscriber { get; set; } = false;
        public bool? IsDelete { get; set; } = false;
        public int? ShopId { get; set; } = null;

        public virtual Shop? Shop { get; set; }

        public virtual ICollection<Cart>? Carts { get; set; }
        public virtual ICollection<Notification>? Notifications { get; set; }
        public virtual ICollection<Order>? Orders { get; set; }
        public virtual ICollection<ProductReport>? ProductReports { get; set; }
        public virtual ICollection<ProductReview>? Reviews { get; set; }
        public virtual ICollection<UserAddress>? UserAddresss { get; set; }
        public virtual ICollection<News>? Newss { get; set; }
        public virtual ICollection<FavoriteProduct>? FavoriteProducts { get; set; }

        public virtual ICollection<Chat>? Senders { get; set; }
        public virtual ICollection<Chat>? Receivers { get; set; }

        public virtual ICollection<ProductInteraction>? Interactions { get; set; }
    }
}
