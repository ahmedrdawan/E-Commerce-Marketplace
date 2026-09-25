using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using E_Commerce.Data.Repositories;
using E_Commerce.ViewModels;
using System.Linq;
using System.Collections.Generic;

namespace E_Commerce.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;

        public HomeController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _categoryRepository.GetAllAsync();
            // create HomeViewModel
            var products = categories.SelectMany(c => c.Products ?? new List<E_Commerce.Entities.Product>());

            var vm = new HomeViewModel
            {
                Categories = categories,
                Products = products
            };

            return View(vm);
        }
    }
}
