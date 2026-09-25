using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using E_Commerce.Data.Repositories;

namespace E_Commerce.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoriesController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _categoryRepository.GetAllAsync();
            return View(categories);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null) return NotFound();
            return View(category);
        }
    }
}
