using E_Commerce.Models.Entities;

namespace E_Commerce.Repositories.Interfaces
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<IReadOnlyList<Order>> GetByCustomerAsync(string customerId);
        Task<IReadOnlyList<OrderItem>> GetItemsBySellerAsync(string sellerId);
        Task<OrderItem?> GetOrderItemAsync(int orderItemId);
    }
}
