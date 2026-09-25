using E_Commerce.Entities;
using System;
using System.Threading.Tasks;

namespace E_Commerce.Data.Repositories
{
    public interface ICustomerRepository
    {
        Task<Customer?> GetByUserIdAsync(Guid userId);
        Task AddAsync(Customer customer);
        void Update(Customer customer);
    }
}
