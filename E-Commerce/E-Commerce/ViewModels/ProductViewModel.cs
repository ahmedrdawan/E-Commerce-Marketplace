using System.ComponentModel.DataAnnotations;
using System;

namespace E_Commerce.ViewModels
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Name { get; set; }

        [Required]
        [StringLength(250)]
        public string Description { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public int AvailableQuantity { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsActive { get; set; }
        [Required]
        public Guid CategoryId { get; set; }
    }
}
