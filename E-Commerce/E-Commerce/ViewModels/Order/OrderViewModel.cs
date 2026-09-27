using E_Commerce.Enums;

namespace E_Commerce.ViewModels.Order
{
    public class OrderViewModel
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        public OrderStatus Status { get; set; }
        public string ShippingAddress { get; set; } = string.Empty;
        public List<OrderItemViewModel> Items { get; set; } = new();
    }
}
