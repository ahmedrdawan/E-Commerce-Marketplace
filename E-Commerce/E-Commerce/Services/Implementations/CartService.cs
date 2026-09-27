using E_Commerce.Models.Entities;
using E_Commerce.Repositories.Interfaces;
using E_Commerce.Services.Interfaces;
using E_Commerce.ViewModels.Cart;

namespace E_Commerce.Services.Implementations
{
    public class CartService : ICartService
    {
        private readonly IUnitOfWork _uow;

        public CartService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<ViewCartViewModel> GetCartAsync(string customerId)
        {
            var items = await _uow.CartItems.GetCartAsync(customerId);

            return new ViewCartViewModel
            {
                Items = items.Select(c => new CartItemViewModel
                {
                    Id = c.Id,
                    ProductId = c.ProductId,
                    ProductName = c.Product?.Name ?? string.Empty,
                    ImageUrl = c.Product?.ImageUrl,
                    UnitPrice = c.Product?.Price ?? 0,
                    Quantity = c.Quantity,
                    AvailableQuantity = c.Product?.AvailableQuantity ?? 0
                }).ToList()
            };
        }

        public async Task<(bool Success, string? Error)> AddToCartAsync(string customerId, int productId, int quantity)
        {
            if (quantity < 1)
                return (false, "Invalid quantity.");

            var product = await _uow.Products.GetByIdAsync(productId);
            if (product is null || product.IsRemovedByAdmin || product.IsDeletedBySeller)
                return (false, "Product is not available.");

            var existing = await _uow.CartItems.GetCartItemAsync(customerId, productId);
            var newQty = (existing?.Quantity ?? 0) + quantity;

            if (newQty > product.AvailableQuantity)
                return (false, $"Only {product.AvailableQuantity} available.");

            if (existing != null)
            {
                existing.Quantity = newQty;
                _uow.CartItems.Update(existing);
            }
            else
            {
                await _uow.CartItems.AddAsync(new CartItem { CustomerId = customerId, ProductId = productId, Quantity = quantity });
            }

            await _uow.CompleteAsync();
            return (true, null);
        }

        public async Task<(bool Success, string? Error)> UpdateQuantityAsync(string customerId, int cartItemId, int quantity)
        {
            var item = await _uow.CartItems.GetByIdAsync(cartItemId);
            if (item is null || item.CustomerId != customerId)
                return (false, "Item not found.");

            if (quantity < 1)
                return (false, "Quantity must be at least 1.");

            var product = await _uow.Products.GetByIdAsync(item.ProductId);
            if (product != null && quantity > product.AvailableQuantity)
                return (false, $"Only {product.AvailableQuantity} available.");

            item.Quantity = quantity;
            _uow.CartItems.Update(item);
            await _uow.CompleteAsync();
            return (true, null);
        }

        public async Task RemoveFromCartAsync(string customerId, int cartItemId)
        {
            var item = await _uow.CartItems.GetByIdAsync(cartItemId);
            if (item is null || item.CustomerId != customerId)
                return;

            _uow.CartItems.Remove(item);
            await _uow.CompleteAsync();
        }
    }
}
