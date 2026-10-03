using E_Commerce.Models.Entities;
using E_Commerce.Services.Interfaces;
using E_Commerce.ViewModels.Order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
    [Authorize(Roles = "Customer")]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly ICartService _cartService;
        private readonly UserManager<ApplicationUser> _userManager;

        public OrderController(IOrderService orderService, ICartService cartService, UserManager<ApplicationUser> userManager)
        {
            _orderService = orderService;
            _cartService = cartService;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Checkout()
        {
            var user = await _userManager.GetUserAsync(User);
            var cart = await _cartService.GetCartAsync(user!.Id);

            if (cart.Items.Count == 0)
            {
                TempData["Error"] = "Your cart is empty.";
                return RedirectToAction("Index", "Cart");
            }

            ViewBag.Cart = cart;
            return View(new CheckoutViewModel { ShippingAddress = user.Address ?? string.Empty });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(CheckoutViewModel viewModel)
        {
            var user = await _userManager.GetUserAsync(User);

            if (!ModelState.IsValid)
            {
                ViewBag.Cart = await _cartService.GetCartAsync(user!.Id);
                return View(viewModel);
            }

            var (success, error, orderId) = await _orderService.PlaceOrderAsync(user!.Id, viewModel);
            if (!success)
            {
                ModelState.AddModelError(string.Empty, error ?? "An unexpected error occurred.");
                ViewBag.Cart = await _cartService.GetCartAsync(user.Id);
                return View(viewModel);
            }

            TempData["Success"] = "Your order has been placed successfully.";
            return RedirectToAction("Details", new { id = orderId });
        }

        public async Task<IActionResult> MyOrders()
        {
            var user = await _userManager.GetUserAsync(User);
            var orders = await _orderService.GetMyOrdersAsync(user!.Id);
            return View(orders);
        }

        public async Task<IActionResult> Details(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var order = await _orderService.GetOrderDetailsAsync(user!.Id, id);
            if (order is null)
                return NotFound();

            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var (success, error) = await _orderService.CancelOrderAsync(user!.Id, id);
            TempData[success ? "Success" : "Error"] = success ? "Order cancelled." : error;
            return RedirectToAction("Details", new { id });
        }
    }
}
