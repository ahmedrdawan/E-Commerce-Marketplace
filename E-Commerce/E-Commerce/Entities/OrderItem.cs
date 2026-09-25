using System;

namespace E_Commerce.Entities
{
    public class OrderItem
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public Guid SellerId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal SubTotal { get; set; }

        public Order Order { get; set; }
        public Product Product { get; set; }
        public Seller Seller { get; set; }
    }
}
