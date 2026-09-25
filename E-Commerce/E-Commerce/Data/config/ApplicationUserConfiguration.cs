using E_Commerce.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_Commerce.Data.config
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.ToTable("ApplicationUsers");
            builder.HasKey(u => u.Id);

            builder.Property(u => u.UserName).HasMaxLength(200);
            builder.Property(u => u.Email).HasMaxLength(320);
            builder.Property(u => u.FullName).HasMaxLength(300);
            builder.Property(u => u.PhoneNumber).HasMaxLength(50);
            builder.Property(u => u.IsActive).HasDefaultValue(true);
            builder.Property(u => u.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.HasOne(u => u.Customer)
                .WithOne(p => p.User)
                .HasForeignKey<Customer>(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(u => u.Seller)
                .WithOne(p => p.User)
                .HasForeignKey<Seller>(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(u => u.UserRoles)
                .WithOne(ur => ur.User)
                .HasForeignKey(ur => ur.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(u => u.SubmittedSellerRequests)
                .WithOne(r => r.User)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(u => u.ReviewedSellerRequests)
                .WithOne(r => r.ReviewedByAdmin)
                .HasForeignKey(r => r.ReviewedByAdminId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
