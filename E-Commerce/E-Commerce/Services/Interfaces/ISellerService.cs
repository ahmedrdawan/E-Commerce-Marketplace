using E_Commerce.Enums;
using E_Commerce.Models.Entities;
using E_Commerce.ViewModels.Dashboard;
using E_Commerce.ViewModels.Product;

namespace E_Commerce.Services.Interfaces
{
    public interface ISellerService
    {
        Task<IReadOnlyList<Product>> GetMyProductsAsync(string sellerId);
        Task<(bool Success, string? Error)> AddProductAsync(string sellerId, CreateProductViewModel viewModel);
        Task<(bool Success, string? Error)> UpdateProductAsync(string sellerId, EditProductViewModel viewModel);
        Task<(bool Success, string? Error)> DeleteProductAsync(string sellerId, int productId);
        Task<(bool Success, string? Error)> UpdateQuantityAsync(string sellerId, int productId, int quantity);
        Task<IReadOnlyList<OrderItem>> GetMyOrderItemsAsync(string sellerId);
        Task<(bool Success, string? Error)> UpdateOrderItemStatusAsync(string sellerId, int orderItemId, OrderStatus status);
        Task<SellerDashboardViewModel> GetDashboardAsync(string sellerId);
    }
}
