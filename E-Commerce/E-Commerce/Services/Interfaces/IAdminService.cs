using E_Commerce.Models.Entities;
using E_Commerce.ViewModels.AdminUser;
using E_Commerce.ViewModels.Dashboard;

namespace E_Commerce.Services.Interfaces
{
    public interface IAdminService
    {
        Task<AdminDashboardViewModel> GetDashboardAsync();
        Task<IReadOnlyList<AdminUserRowViewModel>> GetUsersAsync();
        Task<(bool Success, string? Error)> SetUserActiveAsync(string userId, bool isActive);
        Task<IReadOnlyList<ApplicationUser>> GetPendingSellerRequestsAsync();
        Task<(bool Success, string? Error)> DecideSellerRequestAsync(string userId, bool approve);
        Task<(bool Success, string? Error)> RemoveProductAsync(int productId);
        Task<IReadOnlyList<Product>> GetAllProductsAsync();
    }
}
