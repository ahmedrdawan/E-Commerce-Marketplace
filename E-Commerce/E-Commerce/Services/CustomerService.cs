using E_Commerce.Data.Repositories;
using E_Commerce.Entities;
using E_Commerce.ViewModels;

namespace E_Commerce.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IUnitOfWork _uow;
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(IUnitOfWork uow, ICustomerRepository customerRepository)
        {
            _uow = uow;
            _customerRepository = customerRepository;
        }

        public async Task CreateAsync(CustomerViewModel request)
        {
            var customer = new Customer
            {
                UserId = request.UserId,
                Address = request.Address,
                CreatedAt = DateTime.UtcNow
            };

            await _customerRepository.AddAsync(customer);
            await _uow.SaveChangesAsync();
        }



        public async Task<CustomerInfoViewModel> GetByUserIdAsync(Guid userId)
        {
            var customer = await _customerRepository.GetByUserIdAsync(userId);
            if (customer == null)
                throw new Exception("Customer not found.");

            return new CustomerInfoViewModel
            {
                Id = customer.UserId,
                Address = customer.Address,
                CreatedAt = customer.CreatedAt,
                Email = customer.User.Email,
                UserName = customer.User.UserName
            };
        }

        public async Task UpdateAsync(Guid userId, UpdateCustomerViewModel customer)
        {
            var existing = await _customerRepository.GetByUserIdAsync(userId);

            if (existing == null)
                return;

            existing.Address = customer.Address;

            _customerRepository.Update(existing);

            await _uow.SaveChangesAsync();
        }
    }
}
