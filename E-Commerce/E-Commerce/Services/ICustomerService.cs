

using E_Commerce.ViewModels;

namespace E_Commerce.Services
{
    public interface ICustomerService
    {
        Task<CustomerInfoViewModel> GetByUserIdAsync(Guid userId);
        Task CreateAsync(CustomerViewModel customer);
        Task UpdateAsync(Guid userId, UpdateCustomerViewModel customer);
    }
}
