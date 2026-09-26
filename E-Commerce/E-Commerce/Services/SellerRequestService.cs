using E_Commerce.Data;
using E_Commerce.Entities;
using E_Commerce.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Services
{
    public class SellerRequestService : ISellerRequestService
    {
        //user still has role of customer, but can request to become a seller. Admin can approve or reject the request. If approved, user becomes a seller 
        //add the user to the "Seller" role using UserManager
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly EcommerceDbContext _db;

        public SellerRequestService(EcommerceDbContext db,UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }
        public async Task<SellerRequest> CreateRequestAsync(Guid userId)
        {
            var existingRequest = await _db.SellerRequests
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.RequestedAt)
                .FirstOrDefaultAsync();

            if (existingRequest != null &&
                existingRequest.Status == SellerRequestStatus.Pending)
            {
                throw new InvalidOperationException("You already have a pending seller request.");
            }

            var seller = await _db.Sellers
                .FirstOrDefaultAsync(s => s.UserId == userId);

            if (seller != null && seller.IsApproved)
            {
                throw new InvalidOperationException("You are already an approved seller.");
            }

            var request = new SellerRequest
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Status = SellerRequestStatus.Pending,
                RequestedAt = DateTime.UtcNow
            };

            await _db.SellerRequests.AddAsync(request);
            await _db.SaveChangesAsync();

            return request;
        }

        public async Task<IEnumerable<SellerRequest>> GetPendingRequestsAsync()
        {
            return await _db.SellerRequests
                .Include(r => r.User)
                .Where(r => r.Status == SellerRequestStatus.Pending)
                .OrderBy(r => r.RequestedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<SellerRequest>> GetUserRequestsAsync(Guid userId)
        {
            return await _db.SellerRequests
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.RequestedAt)
                .ToListAsync();
        }

        public async Task<SellerRequest?> GetByIdAsync(Guid id)
        {
            return await _db.SellerRequests
                .Include(r => r.User)
                .Include(r => r.ReviewedByAdmin)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task ApproveAsync(Guid requestId, Guid adminId)
        {
            var request = await _db.SellerRequests.FirstOrDefaultAsync(r => r.Id == requestId);

            if (request == null)
                throw new KeyNotFoundException("Seller request not found.");

            if (request.Status != SellerRequestStatus.Pending)
                throw new InvalidOperationException("This request has already been reviewed.");

            var seller = await _db.Sellers.FirstOrDefaultAsync(s => s.UserId == request.UserId);

            if (seller == null)
            {
                seller = new Seller
                {
                    UserId = request.UserId,
                    IsApproved = true,
                    ApprovedAt = DateTime.UtcNow
                };

                await _db.Sellers.AddAsync(seller);
            }
            else
            {
                seller.IsApproved = true;
                seller.ApprovedAt = DateTime.UtcNow;
            }

            var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == request.UserId);

            if (user == null)
                throw new KeyNotFoundException("User not found.");

            // Add the user to the "Seller" role using UserManager
            await _userManager.AddToRoleAsync(user, "Seller");

            request.Status = SellerRequestStatus.Approved;
            request.ReviewedByAdminId = adminId;
            request.ReviewedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();
        }

        public async Task RejectAsync(Guid requestId, Guid adminId)
        {
            var request = await _db.SellerRequests
                .FirstOrDefaultAsync(r => r.Id == requestId);

            if (request == null)
                throw new KeyNotFoundException("Seller request not found.");

            if (request.Status != SellerRequestStatus.Pending)
                throw new InvalidOperationException(
                    "This request has already been reviewed.");

            request.Status = SellerRequestStatus.Rejected;
            request.ReviewedByAdminId = adminId;
            request.ReviewedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();
        }
    }
}
