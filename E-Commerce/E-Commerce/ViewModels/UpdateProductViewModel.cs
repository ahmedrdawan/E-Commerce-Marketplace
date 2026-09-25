using System.ComponentModel.DataAnnotations;

namespace E_Commerce.ViewModels
{
    public class UpdateProductViewModel
    {
        public string? Name { get; set; }
        [Required]
        public string Description { get; set; }
        public decimal Price { get; set; }
        [Required]
        public int AvailableQuantity { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsActive { get; set; }
    }
}
