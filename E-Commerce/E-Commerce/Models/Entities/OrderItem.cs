using E_Commerce.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Commerce.Models.Entities
{
    public class OrderItem
    {
        public int Id { get; set; }

        public int OrderId { get; set; }
        public Order? Order { get; set; }

        public int ProductId { get; set; }
        public Product? Product { get; set; }

        [Required]
        public string SellerId { get; set; } = string.Empty;
        public ApplicationUser? Seller { get; set; }

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }

        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        [NotMapped]
        public decimal LineTotal => Quantity * UnitPrice;
    }
}
