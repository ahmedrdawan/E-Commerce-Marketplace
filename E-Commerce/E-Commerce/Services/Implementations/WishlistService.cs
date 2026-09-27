using E_Commerce.Models.Entities;
using E_Commerce.Repositories.Interfaces;
using E_Commerce.Services.Interfaces;

namespace E_Commerce.Services.Implementations
{
    public class WishlistService : IWishlistService
    {
        private readonly IUnitOfWork _uow;

        public WishlistService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<IReadOnlyList<Product>> GetWishlistAsync(string customerId)
        {
            var items = await _uow.WishlistItems.GetByCustomerAsync(customerId);
            return items.Where(i => i.Product != null).Select(i => i.Product!).ToList();
        }

        public async Task AddAsync(string customerId, int productId)
        {
            var existing = await _uow.WishlistItems.FindAsync(customerId, productId);
            if (existing != null)
                return;

            await _uow.WishlistItems.AddAsync(new WishlistItem { CustomerId = customerId, ProductId = productId });
            await _uow.CompleteAsync();
        }

        public async Task RemoveAsync(string customerId, int productId)
        {
            var existing = await _uow.WishlistItems.FindAsync(customerId, productId);
            if (existing is null)
                return;

            _uow.WishlistItems.Remove(existing);
            await _uow.CompleteAsync();
        }
    }
}
