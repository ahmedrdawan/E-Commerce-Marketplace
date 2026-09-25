using E_Commerce.Enums;
using System;
using System.Collections.Generic;

namespace E_Commerce.Entities
{
    public class Order
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public decimal TotalPrice { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public Customer Customer { get; set; }
        public ICollection<OrderItem> Items { get; set; }
    }
}
