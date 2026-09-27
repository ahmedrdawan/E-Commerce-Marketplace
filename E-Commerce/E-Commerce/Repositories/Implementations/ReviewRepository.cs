using E_Commerce.Enums;
using E_Commerce.Models.Data;
using E_Commerce.Models.Entities;
using E_Commerce.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Repositories.Implementations
{
    public class ReviewRepository : GenericRepository<Review>, IReviewRepository
    {
        public ReviewRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IReadOnlyList<Review>> GetByProductAsync(int productId) =>
            await _dbSet.Include(r => r.Customer)
                        .Where(r => r.ProductId == productId)
                        .OrderByDescending(r => r.CreatedAt)
                        .ToListAsync();

        public async Task<bool> HasPurchasedAsync(string customerId, int productId) =>
            await _context.OrderItems
                        .Include(oi => oi.Order)
                        .AnyAsync(oi => oi.ProductId == productId
                                     && oi.Order!.CustomerId == customerId
                                     && oi.Status == OrderStatus.Delivered);

        public async Task<bool> HasReviewedAsync(string customerId, int productId) =>
            await _dbSet.AnyAsync(r => r.CustomerId == customerId && r.ProductId == productId);
    }
}
