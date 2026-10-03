using E_Commerce.Models.Entities;

namespace E_Commerce.Repositories.Interfaces
{
    public interface ICartRepository : IGenericRepository<CartItem>
    {
        Task<IReadOnlyList<CartItem>> GetCartAsync(string customerId);
        Task<CartItem?> GetCartItemAsync(string customerId, int productId);
    }
}
