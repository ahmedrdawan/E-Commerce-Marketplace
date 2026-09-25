using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using E_Commerce.Services;
using E_Commerce.ViewModels;
using E_Commerce.Entities;

namespace E_Commerce.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [Authorize(Roles = "Admin,Customer")]
        public async Task<IActionResult> Index()
        {
            var products = await _productService.GetAllAsync();
            return View(products);
        }

        [Authorize(Roles = "Admin,Customer")]
        public async Task<IActionResult> Search(string query)
        {
            if (string.IsNullOrWhiteSpace(query) || query.Length < 3)
            {
                ModelState.AddModelError("query", "Search query must be at least 3 characters long.");
                return View("Index", Array.Empty<Product>());
            }
            var products = await _productService.SearchAsync(query);
            return View("Index", products);
        }

        [Authorize(Roles = "Admin,Customer")]
        public async Task<IActionResult> Details(Guid id)
        {
            var product = await _productService.GetByIdAsync(id);
            if (product == null) return NotFound();
            return View(product);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var product = await _productService.GetByIdAsync(id);
            if (product == null) return NotFound();

            return View(product);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet] 
        public IActionResult Create(Guid categoryId) 
        { 
            var model = new ProductViewModel { CategoryId = categoryId };
            return View(model); 
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken] 
        public async Task<IActionResult> Create(ProductViewModel model) 
        { 
            if (!ModelState.IsValid) 
                return View(model); 
            await _productService.CreateAsync(model); 
            return RedirectToAction(nameof(Index)); 
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, UpdateProductViewModel model)
        {
            if (!ModelState.IsValid) 
                return View(model);

            await _productService.UpdateAsync(id, model);
            return RedirectToAction(nameof(Details), new { id });
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _productService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
