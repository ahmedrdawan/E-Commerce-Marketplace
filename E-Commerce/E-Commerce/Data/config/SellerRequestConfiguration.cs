using E_Commerce.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_Commerce.Data.config
{
    public class SellerRequestConfiguration : IEntityTypeConfiguration<SellerRequest>
    {
        public void Configure(EntityTypeBuilder<SellerRequest> builder)
        {
            builder.HasKey(s => s.Id);
            builder.Property(s => s.RequestedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder
                .HasOne(s => s.User)
                .WithMany(u => u.SubmittedSellerRequests)
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            builder
                .HasOne(s => s.ReviewedByAdmin)
                .WithMany(u => u.ReviewedSellerRequests)
                .HasForeignKey(s => s.ReviewedByAdminId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
