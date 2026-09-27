using E_Commerce.Models.Entities;

namespace E_Commerce.Repositories.Interfaces
{
    public interface IReviewRepository : IGenericRepository<Review>
    {
        Task<IReadOnlyList<Review>> GetByProductAsync(int productId);
        Task<bool> HasPurchasedAsync(string customerId, int productId);
        Task<bool> HasReviewedAsync(string customerId, int productId);
    }
}
