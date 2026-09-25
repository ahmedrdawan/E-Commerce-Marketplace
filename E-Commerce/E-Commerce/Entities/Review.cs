using System;

namespace E_Commerce.Entities
{
    public class Review
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public Guid ProductId { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; }

        public Customer? Customer { get; set; }
        public Product? Product { get; set; }
    }
}
