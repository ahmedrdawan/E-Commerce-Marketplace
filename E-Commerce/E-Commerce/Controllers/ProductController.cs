using E_Commerce.Models.Entities;
using E_Commerce.Services.Interfaces;
using E_Commerce.ViewModels.Product;
using E_Commerce.ViewModels.Review;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IReviewService _reviewService;
        private readonly IWishlistService _wishlistService;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProductController(
            IProductService productService,
            ICategoryService categoryService,
            IReviewService reviewService,
            IWishlistService wishlistService,
            UserManager<ApplicationUser> userManager)
        {
            _productService = productService;
            _categoryService = categoryService;
            _reviewService = reviewService;
            _wishlistService = wishlistService;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index(ProductSearchFilterViewModel filter)
        {
            var (items, total) = await _productService.SearchAsync(filter);

            ViewBag.Categories = await _categoryService.GetAllAsync();
            ViewBag.Filter = filter;
            ViewBag.TotalPages = (int)Math.Ceiling(total / (double)filter.PageSize);

            return View(items);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var product = await _productService.GetDetailsAsync(id);
            if (product is null)
                return NotFound();

            ViewBag.Reviews = await _reviewService.GetForProductAsync(id);

            if (User.Identity?.IsAuthenticated == true)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    var wishlist = await _wishlistService.GetWishlistAsync(user.Id);
                    ViewBag.InWishlist = wishlist.Any(p => p.Id == id);
                }
            }

            return View(product);
        }

        [Authorize(Roles = "Customer")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddReview(CreateReviewViewModel viewModel)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user is null)
                return Challenge();

            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Invalid review data.";
                return RedirectToAction("Details", new { id = viewModel.ProductId });
            }

            var (success, error) = await _reviewService.AddReviewAsync(user.Id, viewModel);
            TempData[success ? "Success" : "Error"] = success ? "Your review has been added successfully." : error;

            return RedirectToAction("Details", new { id = viewModel.ProductId });
        }
    }
}
