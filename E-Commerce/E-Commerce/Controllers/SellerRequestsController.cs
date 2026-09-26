using E_Commerce.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_Commerce.Controllers
{
    [Authorize]
    public class SellerRequestsController : Controller
    {
        private readonly ISellerRequestService _sellerRequestService;

        public SellerRequestsController(
            ISellerRequestService sellerRequestService)
        {
            _sellerRequestService = sellerRequestService;
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePost()
        {
            var userId = GetCurrentUserId();

            try
            {
                await _sellerRequestService.CreateRequestAsync(userId);

                TempData["Success"] = "Your seller request has been submitted successfully.";

                return RedirectToAction(nameof(MyRequests));
            }
            catch (InvalidOperationException ex)
            {
                TempData["Error"] = ex.Message;

                return RedirectToAction(nameof(MyRequests));
            }
        }

        [HttpGet]
        public async Task<IActionResult> MyRequests()
        {
            var userId = GetCurrentUserId();

            var requests =
                await _sellerRequestService.GetUserRequestsAsync(userId);

            return View(requests);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var requests =
                await _sellerRequestService.GetPendingRequestsAsync();

            return View(requests);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(Guid id)
        {
            var adminId = GetCurrentUserId();

            try
            {
                await _sellerRequestService.ApproveAsync(id, adminId);

                TempData["Success"] = "Seller request approved successfully.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reject(Guid id)
        {
            var adminId = GetCurrentUserId();

            try
            {
                await _sellerRequestService.RejectAsync(id, adminId);

                TempData["Success"] = "Seller request rejected successfully.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        private Guid GetCurrentUserId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                throw new UnauthorizedAccessException();

            return Guid.Parse(userId);
        }
    }
}
