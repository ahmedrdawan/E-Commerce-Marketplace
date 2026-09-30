using E_Commerce.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<CartItem> CartItems => Set<CartItem>();
    public DbSet<WishlistItem> WishlistItems => Set<WishlistItem>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<Review> Reviews => Set<Review>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(
            typeof(ApplicationDbContext).Assembly);

        // ADD CATEGORIES ID
        builder.Entity<Category>().HasData(
            new Category
            {
                Id = 1,
                Name = "Electronics",
                Description = "Phones, laptops and gadgets"
            },
            new Category
            {
                Id = 2,
                Name = "Clothing",
                Description = "Men, women and kids fashion"
            },
            new Category
            {
                Id = 3,
                Name = "Home & Kitchen",
                Description = "Appliances and home goods"
            },
            new Category
            {
                Id = 4,
                Name = "Books",
                Description = "Books of all genres"
            },
            new Category
            {
                Id = 5,
                Name = "Sports",
                Description = "Sports and outdoor equipment"
            }
        );

        builder.Entity<OrderItem>()
            .HasOne(oi => oi.Seller)
            .WithMany()
            .HasForeignKey(oi => oi.SellerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Product>()
            .HasOne(p => p.Seller)
            .WithMany(u => u.Products)
            .HasForeignKey(p => p.SellerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Order>()
            .HasOne(o => o.Customer)
            .WithMany(u => u.Orders)
            .HasForeignKey(o => o.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Review>()
            .HasOne(r => r.Customer)
            .WithMany(u => u.Reviews)
            .HasForeignKey(r => r.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<CartItem>()
            .HasIndex(c => new { c.CustomerId, c.ProductId })
            .IsUnique();

        builder.Entity<WishlistItem>()
            .HasIndex(w => new { w.CustomerId, w.ProductId })
            .IsUnique();

        builder.Entity<Product>()
            .HasQueryFilter(p => !p.IsDeletedBySeller);
    }
}
