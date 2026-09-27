using E_Commerce.Enums;
using E_Commerce.Models.Entities;
using E_Commerce.Services.Interfaces;
using E_Commerce.ViewModels.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
    [Authorize(Roles = "Seller")]
    public class SellerController : Controller
    {
        private readonly ISellerService _sellerService;
        private readonly ICategoryService _categoryService;
        private readonly UserManager<ApplicationUser> _userManager;

        public SellerController(ISellerService sellerService, ICategoryService categoryService, UserManager<ApplicationUser> userManager)
        {
            _sellerService = sellerService;
            _categoryService = categoryService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Dashboard()
        {
            var user = await _userManager.GetUserAsync(User);
            var dashboard = await _sellerService.GetDashboardAsync(user!.Id);
            return View(dashboard);
        }

        public async Task<IActionResult> Products()
        {
            var user = await _userManager.GetUserAsync(User);
            var products = await _sellerService.GetMyProductsAsync(user!.Id);
            return View(products);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _categoryService.GetAllAsync();
            return View(new CreateProductViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateProductViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _categoryService.GetAllAsync();
                return View(viewModel);
            }

            var user = await _userManager.GetUserAsync(User);
            var (success, error) = await _sellerService.AddProductAsync(user!.Id, viewModel);

            if (!success)
            {
                ModelState.AddModelError(string.Empty, error ?? "An unexpected error occurred.");
                ViewBag.Categories = await _categoryService.GetAllAsync();
                return View(viewModel);
            }

            TempData["Success"] = "Product added successfully.";
            return RedirectToAction("Products");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var products = await _sellerService.GetMyProductsAsync(user!.Id);
            var product = products.FirstOrDefault(p => p.Id == id);
            if (product is null)
                return NotFound();

            ViewBag.Categories = await _categoryService.GetAllAsync();
            return View(new EditProductViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                AvailableQuantity = product.AvailableQuantity,
                CategoryId = product.CategoryId,
                ExistingImageUrl = product.ImageUrl
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditProductViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _categoryService.GetAllAsync();
                return View(viewModel);
            }

            var user = await _userManager.GetUserAsync(User);
            var (success, error) = await _sellerService.UpdateProductAsync(user!.Id, viewModel);

            if (!success)
            {
                ModelState.AddModelError(string.Empty, error ?? "An unexpected error occurred.");
                ViewBag.Categories = await _categoryService.GetAllAsync();
                return View(viewModel);
            }

            TempData["Success"] = "Product updated successfully.";
            return RedirectToAction("Products");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var (success, error) = await _sellerService.DeleteProductAsync(user!.Id, id);
            TempData[success ? "Success" : "Error"] = success ? "Product deleted." : error;
            return RedirectToAction("Products");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateQuantity(int id, int quantity)
        {
            var user = await _userManager.GetUserAsync(User);
            var (success, error) = await _sellerService.UpdateQuantityAsync(user!.Id, id, quantity);
            TempData[success ? "Success" : "Error"] = success ? "Quantity updated." : error;
            return RedirectToAction("Products");
        }

        public async Task<IActionResult> Orders()
        {
            var user = await _userManager.GetUserAsync(User);
            var items = await _sellerService.GetMyOrderItemsAsync(user!.Id);
            return View(items);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateOrderStatus(int orderItemId, OrderStatus status)
        {
            var user = await _userManager.GetUserAsync(User);
            var (success, error) = await _sellerService.UpdateOrderItemStatusAsync(user!.Id, orderItemId, status);
            TempData[success ? "Success" : "Error"] = success ? "Order status updated." : error;
            return RedirectToAction("Orders");
        }
    }
}
