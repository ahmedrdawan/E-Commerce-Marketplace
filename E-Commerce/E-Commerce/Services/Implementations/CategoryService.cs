using E_Commerce.Models.Entities;
using E_Commerce.Repositories.Interfaces;
using E_Commerce.Services.Interfaces;
using E_Commerce.ViewModels.Category;

namespace E_Commerce.Services.Implementations
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _uow;

        public CategoryService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<IReadOnlyList<Category>> GetAllAsync() => await _uow.Categories.GetAllAsync();

        public async Task<Category?> GetByIdAsync(int id) => await _uow.Categories.GetByIdAsync(id);

        public async Task AddAsync(CategoryViewModel viewModel)
        {
            await _uow.Categories.AddAsync(new Category { Name = viewModel.Name, Description = viewModel.Description });
            await _uow.CompleteAsync();
        }

        public async Task UpdateAsync(CategoryViewModel viewModel)
        {
            var category = await _uow.Categories.GetByIdAsync(viewModel.Id);
            if (category is null)
                return;

            category.Name = viewModel.Name;
            category.Description = viewModel.Description;
            _uow.Categories.Update(category);
            await _uow.CompleteAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var category = await _uow.Categories.GetByIdAsync(id);
            if (category is null)
                return;

            _uow.Categories.Remove(category);
            await _uow.CompleteAsync();
        }
    }
}
