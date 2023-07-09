using BigStore.BusinessObject;
using BigStore.BusinessObject.OtherModels;
using BigStore.Utility;
using Bogus;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BigStore.DataAccess.DbInitializer
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

        public void Initialize()
        {
            try
            {
                if (_db.Database.GetPendingMigrations().Any())
                {
                    _db.Database.Migrate();
                }
            }
            catch { }

            AddDefaultDB();
            AddUserDefault();
            AddCategoryDefault();
            AddProductOfAdminDefault();
        }

        private void AddProductOfAdminDefault()
        {
            var admin = _db.User.FirstOrDefault(x => x.Email == "admin@admin.com");
            // add shop for admin
            if (admin is not null && admin.ShopId is null)
            {
                Shop shop = new()
                {
                    UserId = admin.Id,
                    User = admin,
                    ShopName = "Admin Shop",
                    Description = "Đây là shop của admin",
                    Phone = "01234567889",
                    Address = "Viet Nam",
                    ImageUrl = "/images/shops/shop.jpg",
                    Products = new List<Product>()
                };

                Faker<Product> productDefault = new();
                productDefault.RuleFor(x => x.Shop, shop);
                productDefault.RuleFor(x => x.Name, f => "Product " + f.Lorem.Word());
                productDefault.RuleFor(x => x.Description, f => $"<p>{f.Lorem.Paragraph()}</p>");
                productDefault.RuleFor(x => x.Price, f => decimal.Round(f.Random.Decimal(1, 9999), 2));
                productDefault.RuleFor(x => x.Quantity, f => f.Random.Int(1, 999));

                var categories = _db.Categories.Include(x => x.CategoryChildren).Where(x => x.CategoryChildren.Count == 0).ToList();
                foreach (var cate in categories)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        Product p = productDefault.Generate();
                        p.Category = cate;
                        p.Name = $"{cate.Title} - {p.Name} {j}";
                        p.Slug = Slug.GenerateSlug(p.Name);
                        p.ProductImages = new List<ProductImage>();

                        for (int k = 0; k < 2; k++)
                        {
                            ProductImage productImage = new()
                            {
                                Product = p,
                                ImageUrl = "/images/products/apple.jpg"
                            };
                            //_db.ProductImages.Add(productImage);
                            p.ProductImages.Add(productImage);
                        }
                        shop.Products.Add(p);
                    }
                }
                _db.Shops.Add(shop);
                _db.SaveChanges();
            }
        }

        private void AddCategoryDefault()
        {
            if (!_db.Categories.Any())
            {
                var categorySetDefault = new Faker<Category>();
                categorySetDefault.RuleFor(x => x.Title, f => f.Lorem.Word());
                categorySetDefault.RuleFor(x => x.Description, f => $"<p>Điện thoại: {f.Lorem.Paragraph()}</p>");
                categorySetDefault.RuleFor(x => x.ImageUrl, "/images/categories/fruit.jpg");

                for (int i = 0; i < 4; i++)
                {
                    Category category = categorySetDefault.Generate();
                    category.ParentCategoryId = null;
                    category.Title = $"Category level 1-{i}";
                    category.Slug = Slug.GenerateSlug(category.Title);
                    category.CategoryChildren = new List<Category>();

                    for (var j = 0; j < 3; j++)
                    {
                        Category categoryChild1 = categorySetDefault.Generate();
                        categoryChild1.Title = $"Category level 2-{i}-{j} {categoryChild1.Title}";
                        categoryChild1.Slug = Slug.GenerateSlug(categoryChild1.Title);
                        categoryChild1.ParentCategory = category;
                        categoryChild1.CategoryChildren = new List<Category>();

                        for (var z = 0; z < 2; z++)
                        {
                            Category categoryChild2 = categorySetDefault.Generate();
                            categoryChild2.Title = $"Category level 3-{i}-{j}-{z} {categoryChild2.Title}";
                            categoryChild2.Slug = Slug.GenerateSlug(categoryChild2.Title);
                            categoryChild2.ParentCategory = categoryChild1;

                            categoryChild1.CategoryChildren.Add(categoryChild2);
                        }

                        category.CategoryChildren.Add(categoryChild1);
                    }

                    _db.Categories.Add(category);
                };
                _db.SaveChanges();
            }
        }

        private void AddUserDefault()
        {
            if (!_db.User.Any())
            {
                var admin = new User
                {
                    UserName = "admin@admin.com",
                    FullName = "Administrator",
                    ImageUrl = "/images/avatars/avatar.png",
                    Email = "admin@admin.com",
                    EmailConfirmed = true,
                    ProductSubscriber = true,
                    IsDeleted = false,
                    DOB = DateTime.Now,
                    ShopId = null,
                };
                _userManager.CreateAsync(admin, "Admin123*").GetAwaiter().GetResult();
                _userManager.AddToRolesAsync(admin, new List<string>() { RoleContent.Admin, RoleContent.Seller, RoleContent.Customer }).GetAwaiter().GetResult();

                var userSetDefaults = new Faker<User>();
                userSetDefaults.RuleFor(a => a.UserName, "customerdefault@gmail.com");
                userSetDefaults.RuleFor(a => a.FullName, f => f.Name.FullName());
                userSetDefaults.RuleFor(a => a.Email, "customerdefault@gmail.com");
                userSetDefaults.RuleFor(a => a.EmailConfirmed, true);
                userSetDefaults.RuleFor(a => a.ImageUrl, f => f.Internet.Avatar());

                for (int i = 0; i < 20; i++)
                {
                    User u = userSetDefaults.Generate();
                    u.UserName = i + u.UserName;
                    u.FullName = i + u.FullName;
                    u.Email = i + u.Email;
                    _userManager.CreateAsync(u, "User123*").GetAwaiter().GetResult();
                    _userManager.AddToRoleAsync(u, RoleContent.Customer).GetAwaiter().GetResult();
                }
            }
        }

        private void AddDefaultDB()
        {
            if (!_roleManager.RoleExistsAsync(RoleContent.Admin).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(RoleContent.Admin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(RoleContent.Seller)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(RoleContent.Customer)).GetAwaiter().GetResult();

                List<DiscountType> discountTypes = new()
                {
                    new DiscountType { Name = DiscountTypeContent.ByPercent },
                    new DiscountType { Name = DiscountTypeContent.ByValue }
                };
                _db.DiscountTypes.AddRange(discountTypes);
                _db.SaveChanges();

                List<OrderStatus> orderStatuss = new()
                {
                    new OrderStatus { Name = OrderStatusContent.Ordered },
                    new OrderStatus { Name = OrderStatusContent.DeliveringOrders },
                    new OrderStatus { Name = OrderStatusContent.OrderReceived },
                    new OrderStatus { Name = OrderStatusContent.OrderRefund },
                    new OrderStatus { Name = OrderStatusContent.OrderCanceled }
                };
                _db.OrderStatuses.AddRange(orderStatuss);
                _db.SaveChanges();
            }
        }
    }
}
