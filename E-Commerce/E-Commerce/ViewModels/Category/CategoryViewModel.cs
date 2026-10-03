using System.ComponentModel.DataAnnotations;

namespace E_Commerce.ViewModels.Category
{
    public class CategoryViewModel
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Description { get; set; }
    }
}
