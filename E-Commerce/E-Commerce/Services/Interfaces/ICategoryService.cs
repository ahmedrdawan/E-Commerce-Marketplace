using E_Commerce.Models.Entities;
using E_Commerce.ViewModels.Category;

namespace E_Commerce.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<IReadOnlyList<Category>> GetAllAsync();
        Task<Category?> GetByIdAsync(int id);
        Task AddAsync(CategoryViewModel viewModel);
        Task UpdateAsync(CategoryViewModel viewModel);
        Task DeleteAsync(int id);
    }
}
