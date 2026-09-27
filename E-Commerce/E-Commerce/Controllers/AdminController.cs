using E_Commerce.Services.Interfaces;
using E_Commerce.ViewModels.Category;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;
        private readonly ICategoryService _categoryService;
        private readonly IOrderService _orderService;

        public AdminController(IAdminService adminService, ICategoryService categoryService, IOrderService orderService)
        {
            _adminService = adminService;
            _categoryService = categoryService;
            _orderService = orderService;
        }

        public async Task<IActionResult> Dashboard()
        {
            var dashboard = await _adminService.GetDashboardAsync();
            return View(dashboard);
        }

        public async Task<IActionResult> Users()
        {
            var users = await _adminService.GetUsersAsync();
            return View(users);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleUserActive(string userId, bool activate)
        {
            var (success, error) = await _adminService.SetUserActiveAsync(userId, activate);
            TempData[success ? "Success" : "Error"] = success ? (activate ? "User activated." : "User suspended.") : error;
            return RedirectToAction("Users");
        }

        public async Task<IActionResult> SellerRequests()
        {
            var pending = await _adminService.GetPendingSellerRequestsAsync();
            return View(pending);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DecideSellerRequest(string userId, bool approve)
        {
            var (success, error) = await _adminService.DecideSellerRequestAsync(userId, approve);
            TempData[success ? "Success" : "Error"] = success ? (approve ? "Seller approved." : "Request rejected.") : error;
            return RedirectToAction("SellerRequests");
        }

        public async Task<IActionResult> Categories()
        {
            var categories = await _categoryService.GetAllAsync();
            return View(categories);
        }

        [HttpGet]
        public IActionResult CreateCategory() => View(new CategoryViewModel());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCategory(CategoryViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            await _categoryService.AddAsync(viewModel);
            TempData["Success"] = "Category added.";
            return RedirectToAction("Categories");
        }

        [HttpGet]
        public async Task<IActionResult> EditCategory(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);
            if (category is null)
                return NotFound();

            return View(new CategoryViewModel { Id = category.Id, Name = category.Name, Description = category.Description });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCategory(CategoryViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            await _categoryService.UpdateAsync(viewModel);
            TempData["Success"] = "Category updated.";
            return RedirectToAction("Categories");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            await _categoryService.DeleteAsync(id);
            TempData["Success"] = "Category deleted.";
            return RedirectToAction("Categories");
        }

        public async Task<IActionResult> Products()
        {
            var products = await _adminService.GetAllProductsAsync();
            return View(products);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveProduct(int id)
        {
            var (success, error) = await _adminService.RemoveProductAsync(id);
            TempData[success ? "Success" : "Error"] = success ? "Product removed from the store." : error;
            return RedirectToAction("Products");
        }

        public async Task<IActionResult> Orders()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return View(orders);
        }

        public async Task<IActionResult> OrderDetails(int id)
        {
            var order = await _orderService.GetOrderForAdminAsync(id);
            if (order is null)
                return NotFound();

            return View(order);
        }
    }
}
