using E_Commerce.Entities;
using E_Commerce.Enums;

namespace E_Commerce.Services
{
    public interface ISellerOrderService
    {
        Task<IEnumerable<Order>> GetOrdersAsync(Guid sellerId);

        Task<Order?> GetOrderAsync(Guid orderId, Guid sellerId);

        Task UpdateStatusAsync(Guid orderId, Guid sellerId, OrderStatus status);
    }
}
