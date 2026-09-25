using E_Commerce.Entities;
using E_Commerce.ViewModels;

namespace E_Commerce.Services
{
    public interface ICategoryServices
    {
        Task CreateAsync(CategoryViewModel category);
        Task EditAsync(Guid categoryId, UpdateCategoryViewModel category);
        Task DeleteAsync(Guid categoryId);

        Task<IEnumerable<Category>> GetAllAsync();
        Task<Category> GetByIdAsync(Guid Id);
    }
}
