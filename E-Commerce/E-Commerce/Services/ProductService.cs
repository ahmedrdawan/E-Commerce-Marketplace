
using E_Commerce.Data.Repositories;
using E_Commerce.Entities;
using E_Commerce.ViewModels;

namespace E_Commerce.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _uow;
        private readonly IProductRepository _productRepository;

        public ProductService(
            IUnitOfWork uow,
            IProductRepository productRepository)
        {
            _uow = uow;
            _productRepository = productRepository;
        }

        public async Task CreateAsync(ProductViewModel product)
        {
            if (product.CategoryId == null)
                throw new ArgumentException("Category is required.");

            var existing = await _productRepository.GetByNameAsync(product.Name);

            if (existing != null)
            {
                throw new InvalidOperationException(
                    $"A product with the name '{product.Name}' already exists.");
            }

            var newProduct = new Product
            {
                Id = Guid.NewGuid(),
                CategoryId = product.CategoryId,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                AvailableQuantity = product.AvailableQuantity,
                ImageUrl = product.ImageUrl,
                IsActive = product.IsActive,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _productRepository.AddAsync(newProduct);

            await _uow.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var existing = await _productRepository.GetByIdAsync(id);

            if (existing == null)
                return;

            await _productRepository.DeleteAsync(existing);

            await _uow.SaveChangesAsync();
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _productRepository.GetAllAsync(); 
        }

        public async Task<IEnumerable<Product>> GetByCategoryAsync(Guid categoryId)
        {
            return await _productRepository.GetByCategoryAsync(categoryId);
        }

        public async Task<Product?> GetByIdAsync(Guid id)
        {
            return await _productRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Product>> SearchAsync(string query)
        {
            return await _productRepository.SearchAsync(query);
        }

        public async Task UpdateAsync(
            Guid id,
            UpdateProductViewModel product)
        {
            var existing = await _productRepository.GetByIdAsync(id);

            if (existing == null)
                return;

            var duplicate = await _productRepository.GetByNameAsync(product.Name);

            if (duplicate != null && duplicate.Id != id)
            {
                throw new InvalidOperationException(
                    $"A product with the name '{product.Name}' already exists.");
            }

            existing.Name = product.Name;
            existing.Description = product.Description;
            existing.Price = product.Price;
            existing.AvailableQuantity = product.AvailableQuantity;
            existing.ImageUrl = product.ImageUrl;
            existing.IsActive = product.IsActive;
            existing.UpdatedAt = DateTime.UtcNow;

            await _productRepository.UpdateAsync(existing);

            await _uow.SaveChangesAsync();
        }
    }
}
