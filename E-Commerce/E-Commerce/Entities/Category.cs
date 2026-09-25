using System;
using System.Collections.Generic;

namespace E_Commerce.Entities
{
    public class Category
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
