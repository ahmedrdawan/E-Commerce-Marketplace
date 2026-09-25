using E_Commerce.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace E_Commerce.Data.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {

        private readonly EcommerceDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public CategoryRepository(EcommerceDbContext context, IUnitOfWork unitOfWork)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }
        public async Task AddAsync(Category category)
        {
            await _context.Categories.AddAsync(category);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid categoryId)
        {
            var category = await _context.Categories.FindAsync(categoryId);
            if (category != null)
            {
                // Remove related products first
                var products = await _context.Products.Where(p => p.CategoryId == categoryId).ToListAsync();
                if (products.Any())
                {
                    _context.Products.RemoveRange(products);
                }

                _context.Categories.Remove(category);
                await _unitOfWork.SaveChangesAsync();
            }
        }


        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Category?> GetByIdAsync(Guid categoryId)
        {
            return await _context.Categories
                    .Include(c => c.Products)
                    .FirstOrDefaultAsync(c => c.Id == categoryId);
        }

        public async  Task<Category?> GetByNameAsync(string name)
        {
            return await _context.Categories.FirstOrDefaultAsync(c => c.Name == name);
        }

        public async Task UpdateAsync(Category category)
        {
            _context.Categories.Update(category);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
