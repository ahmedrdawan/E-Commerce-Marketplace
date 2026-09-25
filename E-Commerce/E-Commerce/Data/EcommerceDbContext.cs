using E_Commerce.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Data
{
    public class EcommerceDbContext : IdentityDbContext<ApplicationUser, Role, Guid>
    {
        public EcommerceDbContext(DbContextOptions<EcommerceDbContext> options) : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; } 
        public DbSet<Seller> Sellers { get; set; } 
        public DbSet<SellerRequest> SellerRequests { get; set; } 

        public DbSet<Category> Categories { get; set; } 
        public DbSet<Product> Products { get; set; } 

        public DbSet<Cart> Carts { get; set; } 
        public DbSet<CartItem> CartItems { get; set; } 

        public DbSet<Wishlist> Wishlists { get; set; } 
        public DbSet<WishlistItem> WishlistItems { get; set; } 

        public DbSet<Order> Orders { get; set; } 
        public DbSet<OrderItem> OrderItems { get; set; } 

        public DbSet<Review> Reviews { get; set; } 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(EcommerceDbContext).Assembly);
        }
    }
}
