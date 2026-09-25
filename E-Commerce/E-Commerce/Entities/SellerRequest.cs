using E_Commerce.Enums;
using System;

namespace E_Commerce.Entities
{
  
    public class SellerRequest
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid? ReviewedByAdminId { get; set; }
        public SellerRequestStatus Status { get; set; }
        public DateTime RequestedAt { get; set; }
        public DateTime? ReviewedAt { get; set; }

        public ApplicationUser? User { get; set; }
        public ApplicationUser? ReviewedByAdmin { get; set; }
    }
}
