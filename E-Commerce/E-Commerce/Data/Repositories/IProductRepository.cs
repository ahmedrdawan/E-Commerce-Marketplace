using E_Commerce.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace E_Commerce.Data.Repositories
{
    public interface IProductRepository
    {
        Task<Product?> GetByIdAsync(Guid id);
        Task<Product?> GetByNameAsync(string name);
        Task<IEnumerable<Product>> GetByCategoryAsync(Guid categoryId);
        Task<IEnumerable<Product>> SearchAsync(string query);
        Task AddAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(Product product);
        Task<IEnumerable<Product>> GetAllAsync();
    }
}
