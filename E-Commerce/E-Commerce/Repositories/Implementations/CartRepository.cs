using E_Commerce.Models.Data;
using E_Commerce.Models.Entities;
using E_Commerce.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Repositories.Implementations
{
    public class CartRepository : GenericRepository<CartItem>, ICartRepository
    {
        public CartRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IReadOnlyList<CartItem>> GetCartAsync(string customerId) =>
            await _dbSet.Include(c => c.Product).ThenInclude(p => p!.Seller)
                        .Where(c => c.CustomerId == customerId)
                        .ToListAsync();

        public async Task<CartItem?> GetCartItemAsync(string customerId, int productId) =>
            await _dbSet.Include(c => c.Product)
                        .FirstOrDefaultAsync(c => c.CustomerId == customerId && c.ProductId == productId);
    }
}
