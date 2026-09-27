using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Commerce.Models.Entities
{
    public class Product
    {
        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(2000)]
        public string? Description { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }

        [Range(0, int.MaxValue)]
        public int AvailableQuantity { get; set; }

        [MaxLength(500)]
        public string? ImageUrl { get; set; }

        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        [Required]
        public string SellerId { get; set; } = string.Empty;
        public ApplicationUser? Seller { get; set; }

        public bool IsRemovedByAdmin { get; set; } = false;
        public bool IsDeletedBySeller { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<Review> Reviews { get; set; } = new List<Review>();
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
        public ICollection<WishlistItem> WishlistItems { get; set; } = new List<WishlistItem>();

        [NotMapped]
        public double AverageRating => Reviews != null && Reviews.Count > 0 ? Reviews.Average(r => r.Rating) : 0;

        [NotMapped]
        public bool IsVisible => !IsRemovedByAdmin && !IsDeletedBySeller;
    }
}
