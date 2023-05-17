using BigStore.Models;
using BigStore.Models.OtherModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Composition;

namespace BigStore.Data.DbInitializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbInitializer(ApplicationDbContext db,
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async void Initialize()
        {
            try
            {
                if (_db.Database.GetPendingMigrations().Any())
                {
                    _db.Database.Migrate();
                }
            }
            catch (Exception)
            {

            }

            if (!_roleManager.RoleExistsAsync(RoleContent.Admin).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(RoleContent.Admin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(RoleContent.Seller)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(RoleContent.Customer)).GetAwaiter().GetResult();

                _userManager.CreateAsync(new User
                {
                    UserName = "admin@admin.com",
                    FullName = "Administrator",
                    ImageUrl = "/images/avatars/avatar.png",
                    Email = "admin@admin.com",
                    EmailConfirmed = true,
                    ProductSubscriber = true,
                    IsDelete = false,
                    DOB = DateTime.Now,
                    ShopId = null,
                }, "Admin123*").GetAwaiter().GetResult();

                User? user = _db.User.FirstOrDefault(u => u.Email == "admin@admin.com");
                if (user != null)
                {
                    _userManager.AddToRoleAsync(user, RoleContent.Admin).GetAwaiter().GetResult();
                }
            }

            if (_db.DiscountTypes.Any())
            {
                return;
            }

            List<DiscountType> discounts = new()
            {
                new DiscountType { Name = DiscountTypeContent.ByPercent },
                new DiscountType { Name = DiscountTypeContent.ByValue }
            };
            _db.DiscountTypes.AddRange(discounts);
            _db.SaveChanges();

            List<OrderStatus> orders = new()
            {
                new OrderStatus { Name = OrderStatusContent.Ordered },
                new OrderStatus { Name = OrderStatusContent.DeliveringOrders },
                new OrderStatus { Name = OrderStatusContent.OrderReceived },
                new OrderStatus { Name = OrderStatusContent.OrderRefund },
                new OrderStatus { Name = OrderStatusContent.OrderCanceled }
            };
            //orders.ForEach(order =>
            //{
            _db.OrderStatuses.AddRange(orders);
            _db.SaveChanges();
            //});

            List<ReportStatus> reportStatuses = new()
            {
                new ReportStatus { Name = ReportStatusContent.Pending},
                new ReportStatus { Name = ReportStatusContent.Accept},
                new ReportStatus { Name = ReportStatusContent.Reject}
            };
            //reportStatuses.ForEach(reportStatus =>
            //{
            _db.ReportStatus.AddRange(reportStatuses);
            _db.SaveChanges();
            //});
        }
    }
}
