using Microsoft.AspNetCore.Identity;

namespace BigStore.Modes
{
    public class User : IdentityUser
    {
        public string? FullName { get; set; }
        public DateTime? DOB { get; set; } = DateTime.Now;
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
