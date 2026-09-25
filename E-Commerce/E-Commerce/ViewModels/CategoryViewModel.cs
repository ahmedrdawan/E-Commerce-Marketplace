using E_Commerce.Entities;
using System.ComponentModel.DataAnnotations;

namespace E_Commerce.ViewModels
{
    public class CategoryViewModel
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string? Name { get; set; }
        [StringLength(500)]
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
