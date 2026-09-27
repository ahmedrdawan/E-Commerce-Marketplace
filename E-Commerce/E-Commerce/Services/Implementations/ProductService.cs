using E_Commerce.Models.Entities;
using E_Commerce.Repositories.Interfaces;
using E_Commerce.Services.Interfaces;
using E_Commerce.ViewModels.Product;

namespace E_Commerce.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _uow;

        public ProductService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<(IReadOnlyList<ProductListItemViewModel> Items, int TotalCount)> SearchAsync(ProductSearchFilterViewModel filter)
        {
            var page = filter.Page < 1 ? 1 : filter.Page;
            var pageSize = filter.PageSize is < 1 or > 100 ? 12 : filter.PageSize;

            var products = await _uow.Products.SearchAsync(filter.Keyword, filter.CategoryId, filter.SortByPrice, page, pageSize);
            var total = await _uow.Products.SearchCountAsync(filter.Keyword, filter.CategoryId);

            var items = products.Select(p => new ProductListItemViewModel
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                AvailableQuantity = p.AvailableQuantity,
                ImageUrl = p.ImageUrl,
                CategoryName = p.Category?.Name ?? string.Empty,
                SellerName = p.Seller?.FullName ?? string.Empty,
                AverageRating = p.AverageRating,
                ReviewCount = p.Reviews.Count
            }).ToList();

            return (items, total);
        }

        public async Task<Product?> GetDetailsAsync(int id) =>
            await _uow.Products.FirstOrDefaultAsync(p => p.Id == id && !p.IsRemovedByAdmin,
                includeProperties: "Category,Seller,Reviews,Reviews.Customer");
    }
}
