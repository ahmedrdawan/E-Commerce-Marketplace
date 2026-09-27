using System.ComponentModel.DataAnnotations;

namespace E_Commerce.ViewModels.Product
{
    public class CreateProductViewModel
    {
        [Required, MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(2000)]
        public string? Description { get; set; }

        [Range(0.01, 1000000)]
        public decimal Price { get; set; }

        [Range(0, 1000000)]
        public int AvailableQuantity { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public IFormFile? ImageFile { get; set; }
    }
}
