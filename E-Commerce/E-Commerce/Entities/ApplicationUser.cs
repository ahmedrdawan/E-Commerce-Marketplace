using Microsoft.AspNetCore.Identity;

namespace E_Commerce.Entities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string? FullName { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }

        public Customer? Customer { get; set; }
        public Seller? Seller { get; set; }
        public ICollection<UserRole>? UserRoles { get; set; }
        public ICollection<SellerRequest>? SubmittedSellerRequests { get; set; }
        public ICollection<SellerRequest>? ReviewedSellerRequests { get; set; }
    }
}
