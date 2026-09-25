using E_Commerce.Data;
using E_Commerce.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace E_Commerce.Data.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly EcommerceDbContext _db;

        public CustomerRepository(EcommerceDbContext db)
        {
            _db = db;
        }

        public async Task AddAsync(Customer customer)
        {
            await _db.Customers.AddAsync(customer);
        }

        public async Task<Customer?> GetByUserIdAsync(Guid userId)
        {
            return await _db.Customers
                .Include(c => c.User)
                .Include(c => c.Cart)
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }

        public void Update(Customer customer)
        {
            _db.Customers.Update(customer);
        }
    }
}
