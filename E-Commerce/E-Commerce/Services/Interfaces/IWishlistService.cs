using E_Commerce.Models.Entities;

namespace E_Commerce.Services.Interfaces
{
    public interface IWishlistService
    {
        Task<IReadOnlyList<Product>> GetWishlistAsync(string customerId);
        Task AddAsync(string customerId, int productId);
        Task RemoveAsync(string customerId, int productId);
    }
}
