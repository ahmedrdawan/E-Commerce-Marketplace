using E_Commerce.Models.Entities;

namespace E_Commerce.Repositories.Interfaces
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<IReadOnlyList<Product>> SearchAsync(string? keyword, int? categoryId, string? sortByPrice, int page, int pageSize);
        Task<int> SearchCountAsync(string? keyword, int? categoryId);
        Task<IReadOnlyList<Product>> GetBySellerAsync(string sellerId);
    }
}
