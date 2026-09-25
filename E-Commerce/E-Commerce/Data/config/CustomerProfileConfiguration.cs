using E_Commerce.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_Commerce.Data.config
{
    public class CustomerProfileConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.HasKey(c => c.UserId);
            builder.Property(c => c.Address).HasMaxLength(1000);
            builder.Property(c => c.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder
                .HasOne(c => c.User)
                .WithOne(u => u.Customer)
                .HasForeignKey<Customer>(c => c.UserId);

            builder
                .HasOne(c => c.Cart)
                .WithOne(cart => cart.Customer)
                .HasForeignKey<Cart>(cart => cart.CustomerId);
        }
    }
}
