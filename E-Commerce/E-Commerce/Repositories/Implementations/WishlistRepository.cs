using E_Commerce.Models.Data;
using E_Commerce.Models.Entities;
using E_Commerce.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Repositories.Implementations
{
    public class WishlistRepository : GenericRepository<WishlistItem>, IWishlistRepository
    {
        public WishlistRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IReadOnlyList<WishlistItem>> GetByCustomerAsync(string customerId) =>
            await _dbSet.Include(w => w.Product).ThenInclude(p => p!.Category)
                        .Where(w => w.CustomerId == customerId)
                        .ToListAsync();

        public async Task<WishlistItem?> FindAsync(string customerId, int productId) =>
            await _dbSet.FirstOrDefaultAsync(w => w.CustomerId == customerId && w.ProductId == productId);
    }
}
