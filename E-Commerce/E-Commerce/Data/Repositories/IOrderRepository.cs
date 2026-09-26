using E_Commerce.Entities;

namespace E_Commerce.Data.Repositories
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetOrdersBySellerIdAsync(Guid sellerId);

        Task<Order?> GetOrderByIdForSellerAsync(Guid orderId, Guid sellerId);

        Task UpdateAsync(Order order);
    }
}
