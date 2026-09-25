using E_Commerce.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_Commerce.Data.config
{
    public class WishlistConfiguration : IEntityTypeConfiguration<Wishlist>
    {
        public void Configure(EntityTypeBuilder<Wishlist> builder)
        {
            builder.HasKey(w => w.Id);
            builder.Property(w => w.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder
                .HasMany(w => w.Items)
                .WithOne(i => i.Wishlist)
                .HasForeignKey(i => i.WishlistId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
