using E_Commerce.Enums;
using E_Commerce.Models.Entities;
using E_Commerce.Repositories.Interfaces;
using E_Commerce.Services.Interfaces;
using E_Commerce.ViewModels.AdminUser;
using E_Commerce.ViewModels.Dashboard;
using Microsoft.AspNetCore.Identity;

namespace E_Commerce.Services.Implementations
{
    public class AdminService : IAdminService
    {
        private readonly IUnitOfWork _uow;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminService(IUnitOfWork uow, UserManager<ApplicationUser> userManager)
        {
            _uow = uow;
            _userManager = userManager;
        }

        public async Task<AdminDashboardViewModel> GetDashboardAsync()
        {
            var customers = await _userManager.GetUsersInRoleAsync("Customer");
            var sellers = await _userManager.GetUsersInRoleAsync("Seller");
            var allOrders = await _uow.Orders.GetAllAsync();
            var allProducts = await _uow.Products.GetAllAsync();

            return new AdminDashboardViewModel
            {
                TotalCustomers = customers.Count,
                TotalSellers = sellers.Count,
                TotalProducts = allProducts.Count(p => !p.IsRemovedByAdmin),
                TotalOrders = allOrders.Count,
                PendingOrders = allOrders.Count(o => o.Status == OrderStatus.Pending)
            };
        }

        public async Task<IReadOnlyList<AdminUserRowViewModel>> GetUsersAsync()
        {
            var users = _userManager.Users.ToList();
            var result = new List<AdminUserRowViewModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                result.Add(new AdminUserRowViewModel
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    Email = user.Email ?? string.Empty,
                    IsActive = user.IsActive,
                    SellerStatus = user.SellerStatus,
                    Roles = roles
                });
            }

            return result;
        }

        public async Task<(bool Success, string? Error)> SetUserActiveAsync(string userId, bool isActive)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
                return (false, "User not found.");

            user.IsActive = isActive;
            user.LockoutEnabled = true;
            user.LockoutEnd = isActive ? null : DateTimeOffset.MaxValue;

            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded ? (true, null) : (false, string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        public async Task<IReadOnlyList<ApplicationUser>> GetPendingSellerRequestsAsync() =>
            _userManager.Users.Where(u => u.SellerStatus == SellerRequestStatus.Pending).ToList();

        public async Task<(bool Success, string? Error)> DecideSellerRequestAsync(string userId, bool approve)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
                return (false, "User not found.");

            user.SellerStatus = approve ? SellerRequestStatus.Approved : SellerRequestStatus.Rejected;
            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
                return (false, string.Join(", ", updateResult.Errors.Select(e => e.Description)));

            if (approve && !await _userManager.IsInRoleAsync(user, "Seller"))
                await _userManager.AddToRoleAsync(user, "Seller");

            return (true, null);
        }

        public async Task<(bool Success, string? Error)> RemoveProductAsync(int productId)
        {
            var product = await _uow.Products.GetByIdAsync(productId);
            if (product is null)
                return (false, "Product not found.");

            product.IsRemovedByAdmin = true;
            _uow.Products.Update(product);
            await _uow.CompleteAsync();
            return (true, null);
        }

        public async Task<IReadOnlyList<Product>> GetAllProductsAsync() =>
            await _uow.Products.FindAsync(orderBy: q => q.OrderByDescending(p => p.CreatedAt), includeProperties: "Category,Seller");
    }
}
