using System;
using System.Collections.Generic;

namespace E_Commerce.Entities
{
    public class Cart
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public Customer? Customer { get; set; }
        public ICollection<CartItem>? Items { get; set; }
    }
}
