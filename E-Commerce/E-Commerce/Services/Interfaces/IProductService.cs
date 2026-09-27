using E_Commerce.Models.Entities;
using E_Commerce.ViewModels.Product;

namespace E_Commerce.Services.Interfaces
{
    public interface IProductService
    {
        Task<(IReadOnlyList<ProductListItemViewModel> Items, int TotalCount)> SearchAsync(ProductSearchFilterViewModel filter);
        Task<Product?> GetDetailsAsync(int id);
    }
}
