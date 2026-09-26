using E_Commerce.Data;
using E_Commerce.Enums;
using E_Commerce.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Controllers
{
    [Authorize(Roles = "Seller")]
    public class SellerOrdersController: Controller
    {
        private readonly ISellerOrderService _sellerOrderService;
        private readonly EcommerceDbContext _db;

        public SellerOrdersController(ISellerOrderService sellerOrderService,EcommerceDbContext db)
        {
            _sellerOrderService = sellerOrderService;
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var sellerId = await GetApprovedSellerIdAsync();

            if (sellerId == null)
                return Forbid();

            var orders =
                await _sellerOrderService
                    .GetOrdersAsync(sellerId.Value);

            return View(orders);
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var sellerId = await GetApprovedSellerIdAsync();

            if (sellerId == null)
                return Forbid();

            var order =
                await _sellerOrderService.GetOrderAsync( id, sellerId.Value);

            if (order == null)
                return NotFound();

            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus( Guid id,OrderStatus status)
        {
            var sellerId = await GetApprovedSellerIdAsync();

            if (sellerId == null)
                return Forbid();

            try
            {
                await _sellerOrderService.UpdateStatusAsync( id, sellerId.Value, status);

                TempData["Success"] = "Order status updated successfully.";
            }
            catch (InvalidOperationException ex)
            {
                TempData["Error"] = ex.Message;
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }

            return RedirectToAction(
                nameof(Details),
                new { id });
        }

        private async Task<Guid?> GetApprovedSellerIdAsync()
        {
            var userIdValue = User.FindFirstValue( ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdValue))
                return null;

            if (!Guid.TryParse( userIdValue, out var userId))
                return null;

            var seller = await _db.Sellers .FirstOrDefaultAsync(
                    s => s.UserId == userId &&
                         s.IsApproved);

            return seller?.UserId;
        }
    }
}
