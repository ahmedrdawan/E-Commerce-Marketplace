using E_Commerce.Services;
using E_Commerce.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
    [Authorize]
    public class CustomersController : Controller
    {
        private readonly ICustomerService _customerService;
        private readonly IAuthService _authService;

        public CustomersController(ICustomerService customerService, IAuthService authService)
        {
            _customerService = customerService;
            _authService = authService;
        }


       public async Task<IActionResult> Profile()
        {
            var userId = await _authService.GetCurrentUser();
            var customerInfo = await _customerService.GetByUserIdAsync(userId);

            if (customerInfo == null) return NotFound();

            return View(customerInfo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(UpdateCustomerViewModel model)
        {
            if (!ModelState.IsValid) 
                return View("Profile", model);

            var userId = await _authService.GetCurrentUser();

            await _customerService.UpdateAsync(userId, model);

            return RedirectToAction(nameof(Profile));
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
