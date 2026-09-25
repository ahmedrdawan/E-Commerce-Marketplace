using E_Commerce.Data.Repositories;
using E_Commerce.Entities;
using E_Commerce.ViewModels;

namespace E_Commerce.Services
{
    public class CategoryServices : ICategoryServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICategoryRepository _categoryRepository;

        public CategoryServices(IUnitOfWork unitOfWork, ICategoryRepository categoryRepository)
        {
            _unitOfWork = unitOfWork;
            _categoryRepository = categoryRepository;
        }

        public async Task CreateAsync(CategoryViewModel category)
        {
            var existingCategory = await _categoryRepository.GetByNameAsync(category.Name);

            if (existingCategory != null)
            {
                throw new Exception("Category with the same name already exists.");
            }

            var newCategory = new Category
            {
                Id = Guid.NewGuid(),
                Name = category.Name,
                Description = category.Description,
                CreatedAt = DateTime.UtcNow
            };

            await _categoryRepository.AddAsync(newCategory);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid categoryId)
        {
            var category = await _categoryRepository.GetByIdAsync(categoryId);
            if (category == null)
            {
                throw new Exception("Category not found.");
            }

            await _categoryRepository.DeleteAsync(categoryId);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task EditAsync(Guid categoryId, UpdateCategoryViewModel category)
        {
            var existingCategory = await _categoryRepository.GetByIdAsync(categoryId);
            if (existingCategory == null)
            {
                throw new Exception("Category not found.");
            }

            existingCategory.Name = category.Name;
            existingCategory.Description = category.Description;

            await _categoryRepository.UpdateAsync(existingCategory);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _categoryRepository.GetAllAsync();
        }

        public async Task<Category> GetByIdAsync(Guid Id)
        {
            return await _categoryRepository.GetByIdAsync(Id);

        }
    }
}
