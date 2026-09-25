using System;

namespace E_Commerce.Entities
{
    public class Customer
    {
        public Guid UserId { get; set; }
        public string? Address { get; set; }
        public DateTime CreatedAt { get; set; }

        public ApplicationUser? User { get; set; }

        public Cart? Cart { get; set; }
    }
}
