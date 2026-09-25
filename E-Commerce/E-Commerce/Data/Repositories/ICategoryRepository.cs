using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using E_Commerce.Entities;

namespace E_Commerce.Data.Repositories
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllAsync();
        Task<Category> GetByIdAsync(Guid id);
        Task AddAsync(Category category);
        Task UpdateAsync(Category category);
        Task DeleteAsync(Guid Id);

        Task<Category?> GetByNameAsync(string name);

    }
}
