using E_Commerce.Models.Entities;
using E_Commerce.ViewModels.Order;

namespace E_Commerce.Services.Interfaces
{
    public interface IOrderService
    {
        Task<(bool Success, string? Error, int? OrderId)> PlaceOrderAsync(string customerId, CheckoutViewModel viewModel);
        Task<IReadOnlyList<OrderViewModel>> GetMyOrdersAsync(string customerId);
        Task<OrderViewModel?> GetOrderDetailsAsync(string customerId, int orderId);
        Task<(bool Success, string? Error)> CancelOrderAsync(string customerId, int orderId);
        Task<IReadOnlyList<Order>> GetAllOrdersAsync();
        Task<Order?> GetOrderForAdminAsync(int orderId);
    }
}
