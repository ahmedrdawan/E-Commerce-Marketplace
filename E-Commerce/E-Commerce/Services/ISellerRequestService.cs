using E_Commerce.Entities;

namespace E_Commerce.Services
{
    public interface ISellerRequestService
    {
        Task<SellerRequest> CreateRequestAsync(Guid userId);

        Task<IEnumerable<SellerRequest>> GetPendingRequestsAsync();

        Task<IEnumerable<SellerRequest>> GetUserRequestsAsync(Guid userId);

        Task<SellerRequest?> GetByIdAsync(Guid id);

        Task ApproveAsync(Guid requestId, Guid adminId);

        Task RejectAsync(Guid requestId, Guid adminId);
    }
}
