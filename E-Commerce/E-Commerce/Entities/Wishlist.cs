using System;
using System.Collections.Generic;

namespace E_Commerce.Entities
{
    public class Wishlist
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public DateTime CreatedAt { get; set; }

        public Customer Customer { get; set; }
        public ICollection<WishlistItem> Items { get; set; }
    }
}
