using E_Commerce.Entities;
using E_Commerce.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace E_Commerce.Services
{
    public interface IProductService
    {
        Task<Product?> GetByIdAsync(Guid id);
        Task<IEnumerable<Product>> SearchAsync(string query);
        Task<IEnumerable<Product>> GetByCategoryAsync(Guid categoryId);
        Task CreateAsync(ProductViewModel product);
        Task UpdateAsync(Guid id, UpdateProductViewModel product);
        Task DeleteAsync(Guid id);

        Task<IEnumerable<Product>> GetAllAsync();
    }
}
