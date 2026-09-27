using E_Commerce.ViewModels.Cart;

namespace E_Commerce.Services.Interfaces
{
    public interface ICartService
    {
        Task<ViewCartViewModel> GetCartAsync(string customerId);
        Task<(bool Success, string? Error)> AddToCartAsync(string customerId, int productId, int quantity);
        Task<(bool Success, string? Error)> UpdateQuantityAsync(string customerId, int cartItemId, int quantity);
        Task RemoveFromCartAsync(string customerId, int cartItemId);
    }
}
