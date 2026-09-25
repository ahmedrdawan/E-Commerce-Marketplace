using E_Commerce.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_Commerce.Data.config
{
    public class SellerProfileConfiguration : IEntityTypeConfiguration<Seller>
    {
        public void Configure(EntityTypeBuilder<Seller> builder)
        {
            builder.HasKey(s => s.UserId);
            builder.Property(s => s.StoreName).HasMaxLength(300);
            builder.Property(s => s.StoreDescription).HasMaxLength(2000);
            builder.Property(s => s.StoreLogo).HasMaxLength(1000);
            builder.Property(s => s.Address).HasMaxLength(1000);

            builder
                .HasOne(s => s.User)
                .WithOne(u => u.Seller)
                .HasForeignKey<Seller>(s => s.UserId);

            builder
                .HasMany(s => s.Products)
                .WithOne(p => p.Seller)
                .HasForeignKey(p => p.SellerId)
                .OnDelete(DeleteBehavior.Cascade);
            builder
                .HasMany(s => s.OrderItems)
                .WithOne(oi => oi.Seller)
                .HasForeignKey(oi => oi.SellerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
