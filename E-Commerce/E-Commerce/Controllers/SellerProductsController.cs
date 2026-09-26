using E_Commerce.Data;
using E_Commerce.Services;
using E_Commerce.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Controllers
{
    [Authorize(Roles = "Seller")]
    public class SellerProductsController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryServices _categoryServices;
        private readonly EcommerceDbContext _db;

        public SellerProductsController(IProductService productService,ICategoryServices categoryServices, EcommerceDbContext db)
        {
            _productService = productService;
            _categoryServices = categoryServices;
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var sellerId = await GetApprovedSellerIdAsync();

            if (sellerId == null)
                return Forbid();

            var products =
                await _productService.GetBySellerIdAsync(
                    sellerId.Value);

            return View(products);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var sellerId = await GetApprovedSellerIdAsync();

            if (sellerId == null)
                return Forbid();

            await LoadCategories();

            return View(new ProductViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            ProductViewModel model)
        {
            var sellerId = await GetApprovedSellerIdAsync();

            if (sellerId == null)
                return Forbid();

            if (!ModelState.IsValid)
            {
                await LoadCategories();
                return View(model);
            }

            try
            {
                await _productService.CreateAsync(model,sellerId.Value);

                TempData["Success"] ="Product created successfully.";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty,ex.Message);

                await LoadCategories();

                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var sellerId = await GetApprovedSellerIdAsync();

            if (sellerId == null)
                return Forbid();

            var product =
                await _productService.GetByIdAsync(id);

            if (product == null)
                return NotFound();

            if (product.SellerId != sellerId)
                return Forbid();

            var model = new UpdateProductViewModel
            {
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                AvailableQuantity = product.AvailableQuantity,
                ImageUrl = product.ImageUrl,
                IsActive = product.IsActive
            };

            ViewBag.ProductId = id;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            Guid id,
            UpdateProductViewModel model)
        {
            var sellerId = await GetApprovedSellerIdAsync();

            if (sellerId == null)
                return Forbid();

            if (!ModelState.IsValid)
            {
                ViewBag.ProductId = id;
                return View(model);
            }

            try
            {
                await _productService.UpdateAsync( id,model,sellerId.Value);

                TempData["Success"] ="Product updated successfully.";

                return RedirectToAction(nameof(Index));
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(
                    string.Empty,
                    ex.Message);

                ViewBag.ProductId = id;

                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            var sellerId = await GetApprovedSellerIdAsync();

            if (sellerId == null)
                return Forbid();

            try
            {
                await _productService.DeleteAsync( id, sellerId.Value);

                TempData["Success"] ="Product deleted successfully.";
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task<Guid?> GetApprovedSellerIdAsync()
        {
            var userIdValue =User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdValue))
                return null;

            if (!Guid.TryParse( userIdValue, out var userId))
            {
                return null;
            }

            var seller = await _db.Sellers
                .FirstOrDefaultAsync(
                    s => s.UserId == userId &&
                         s.IsApproved);

            return seller?.UserId;
        }

        private async Task LoadCategories()
        {
            var categories =await _categoryServices.GetAllAsync();

            ViewBag.Categories = categories;
        }
    }
}
