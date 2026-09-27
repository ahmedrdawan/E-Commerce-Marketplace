using E_Commerce.Models.Data;
using E_Commerce.Models.Entities;
using E_Commerce.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Repositories.Implementations
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext context) : base(context)
        {
        }

        private IQueryable<Product> VisibleQuery() =>
            _dbSet.Include(p => p.Category)
                .Include(p => p.Seller)
                .Include(p => p.Reviews)
                .Where(p => !p.IsRemovedByAdmin);

        public async Task<IReadOnlyList<Product>> SearchAsync(string? keyword, int? categoryId, string? sortByPrice, int page, int pageSize)
        {
            var query = VisibleQuery();

            if (!string.IsNullOrWhiteSpace(keyword))
                query = query.Where(p => p.Name.Contains(keyword) || (p.Description != null && p.Description.Contains(keyword)));

            if (categoryId.HasValue)
                query = query.Where(p => p.CategoryId == categoryId.Value);

            query = sortByPrice?.ToLower() switch
            {
                "asc" => query.OrderBy(p => p.Price),
                "desc" => query.OrderByDescending(p => p.Price),
                _ => query.OrderByDescending(p => p.CreatedAt)
            };

            return await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        }

        public async Task<int> SearchCountAsync(string? keyword, int? categoryId)
        {
            var query = VisibleQuery();

            if (!string.IsNullOrWhiteSpace(keyword))
                query = query.Where(p => p.Name.Contains(keyword) || (p.Description != null && p.Description.Contains(keyword)));

            if (categoryId.HasValue)
                query = query.Where(p => p.CategoryId == categoryId.Value);

            return await query.CountAsync();
        }

        public async Task<IReadOnlyList<Product>> GetBySellerAsync(string sellerId) =>
            await _dbSet.Include(p => p.Category)
                        .Where(p => p.SellerId == sellerId)
                        .OrderByDescending(p => p.CreatedAt)
                        .ToListAsync();
    }
}
