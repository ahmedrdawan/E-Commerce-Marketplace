using E_Commerce.Enums;

namespace E_Commerce.ViewModels.Order
{
    public class OrderItemViewModel
    {
        public int Id { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string? SellerName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public OrderStatus Status { get; set; }
        public decimal LineTotal => Quantity * UnitPrice;
    }
}
