using E_Commerce.Models.Entities;

namespace E_Commerce.Repositories.Interfaces
{
    public interface IWishlistRepository : IGenericRepository<WishlistItem>
    {
        Task<IReadOnlyList<WishlistItem>> GetByCustomerAsync(string customerId);
        Task<WishlistItem?> FindAsync(string customerId, int productId);
    }
}
