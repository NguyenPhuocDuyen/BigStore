﻿using BigStore.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace BigStore.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Cart> Carts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<DiscountCode> DiscountCodes { get; set; }
        public DbSet<DiscountType> DiscountTypes { get; set; }
        public DbSet<FavoriteProduct> FavoriteProducts { get; set; }
        public DbSet<News> Newss { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<OrderStatus> OrderStatuses { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<ProductInteraction> ProductInteractions { get; set; }
        public DbSet<ProductInteractionStatus> ProductInteractionStatuses { get; set; }
        public DbSet<ProductReport> ProductReports { get; set; }
        public DbSet<ProductReview> ProductReviews { get; set; }
        public DbSet<ReportProductReview> ReportProductReviews { get; set; }
        public DbSet<ReportStatus> ReportStatus { get; set; }
        public DbSet<Shop> Shops { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<UserAddress> UserAddresses { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Shop>()
                .HasOne(s => s.User)
                .WithOne(u => u.Shop)
                .HasForeignKey<User>(u => u.ShopId);

            builder.Entity<Category>()
               .HasIndex(b => b.Slug)
               .IsUnique();

            builder.Entity<Product>()
               .HasIndex(b => b.Slug)
               .IsUnique();

            builder.Entity<DiscountCode>()
               .HasIndex(b => b.Code)
               .IsUnique();

            builder.Entity<Shop>()
               .HasIndex(b => b.ShopName)
               .IsUnique();

            builder.Entity<Chat>()
                    .HasOne(c => c.Receiver)
                    .WithMany(u => u.Receivers)
                    .HasForeignKey(c => c.ReceiverId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Chat>()
                .HasOne(c => c.Sender)
                .WithMany(u => u.Senders)
                .HasForeignKey(c => c.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                var tableName = entityType.GetTableName();
                if (tableName.StartsWith("AspNet"))
                {
                    entityType.SetTableName(tableName.Substring(6));
                }
            }
        }
    }
}