using E_Commerce.Data;
using E_Commerce.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_Commerce.Data.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly EcommerceDbContext _db;

        public ProductRepository(EcommerceDbContext db)
        {
            _db = db;
        }

        public async Task AddAsync(Product product)
        {
            await _db.Products.AddAsync(product);
        }

        public async Task<IEnumerable<Product>> GetByCategoryAsync(Guid categoryId)
        {
            return await _db.Products
                .Where(p => p.CategoryId == categoryId && p.IsActive)
                .ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(Guid id)
        {
            return await _db.Products
                .Include(p => p.Seller)
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _db.Products
                .Include(p => p.Category)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> SearchAsync(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return Enumerable.Empty<Product>();

            query = query.Trim();
            return await _db.Products
                .Where(p => p.IsActive && p.Name!.ToLower().Contains(query))
                .ToListAsync();
        }

        public async Task DeleteAsync(Product product)
        {
            _db.Products.Remove(product);
        }

        public async Task UpdateAsync(Product product)
        {
            _db.Products.Update(product);
        }

        public async Task<Product?> GetByNameAsync(string name)
        {
            return await _db.Products
                .FirstOrDefaultAsync(p => p.Name == name);
        }

        // Additional method to get products by seller ID 
        public async Task<IEnumerable<Product>> GetBySellerIdAsync(Guid sellerId)
        {
            return await _db.Products
                .Include(p => p.Category)
                .Where(p => p.SellerId == sellerId)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }
    }
}
