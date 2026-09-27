using E_Commerce.Enums;
using E_Commerce.Models.Entities;
using E_Commerce.Repositories.Interfaces;
using E_Commerce.Services.Interfaces;
using E_Commerce.ViewModels.Dashboard;
using E_Commerce.ViewModels.Product;

namespace E_Commerce.Services.Implementations
{
    public class SellerService : ISellerService
    {
        private readonly IUnitOfWork _uow;
        private readonly IFileStorageService _fileStorage;

        public SellerService(IUnitOfWork uow, IFileStorageService fileStorage)
        {
            _uow = uow;
            _fileStorage = fileStorage;
        }

        public async Task<IReadOnlyList<Product>> GetMyProductsAsync(string sellerId) =>
            await _uow.Products.GetBySellerAsync(sellerId);

        public async Task<(bool Success, string? Error)> AddProductAsync(string sellerId, CreateProductViewModel viewModel)
        {
            string? imageUrl;
            try
            {
                imageUrl = await _fileStorage.SaveProductImageAsync(viewModel.ImageFile);
            }
            catch (InvalidOperationException ex)
            {
                return (false, ex.Message);
            }

            var product = new Product
            {
                Name = viewModel.Name,
                Description = viewModel.Description,
                Price = viewModel.Price,
                AvailableQuantity = viewModel.AvailableQuantity,
                CategoryId = viewModel.CategoryId,
                SellerId = sellerId,
                ImageUrl = imageUrl
            };

            await _uow.Products.AddAsync(product);
            await _uow.CompleteAsync();
            return (true, null);
        }

        public async Task<(bool Success, string? Error)> UpdateProductAsync(string sellerId, EditProductViewModel viewModel)
        {
            var product = await _uow.Products.GetByIdAsync(viewModel.Id);
            if (product is null || product.SellerId != sellerId)
                return (false, "Product not found or you do not have permission to edit it.");

            if (viewModel.ImageFile != null)
            {
                string? newImage;
                try
                {
                    newImage = await _fileStorage.SaveProductImageAsync(viewModel.ImageFile);
                }
                catch (InvalidOperationException ex)
                {
                    return (false, ex.Message);
                }

                _fileStorage.DeleteProductImage(product.ImageUrl);
                product.ImageUrl = newImage;
            }

            product.Name = viewModel.Name;
            product.Description = viewModel.Description;
            product.Price = viewModel.Price;
            product.AvailableQuantity = viewModel.AvailableQuantity;
            product.CategoryId = viewModel.CategoryId;

            _uow.Products.Update(product);
            await _uow.CompleteAsync();
            return (true, null);
        }

        public async Task<(bool Success, string? Error)> DeleteProductAsync(string sellerId, int productId)
        {
            var product = await _uow.Products.GetByIdAsync(productId);
            if (product is null || product.SellerId != sellerId)
                return (false, "Product not found or you do not have permission to delete it.");

            product.IsDeletedBySeller = true; // soft delete keeps order history intact
            _uow.Products.Update(product);
            await _uow.CompleteAsync();
            return (true, null);
        }

        public async Task<(bool Success, string? Error)> UpdateQuantityAsync(string sellerId, int productId, int quantity)
        {
            if (quantity < 0)
                return (false, "Invalid quantity.");

            var product = await _uow.Products.GetByIdAsync(productId);
            if (product is null || product.SellerId != sellerId)
                return (false, "Product not found or you do not have permission to edit it.");

            product.AvailableQuantity = quantity;
            _uow.Products.Update(product);
            await _uow.CompleteAsync();
            return (true, null);
        }

        public async Task<IReadOnlyList<OrderItem>> GetMyOrderItemsAsync(string sellerId) =>
            await _uow.Orders.GetItemsBySellerAsync(sellerId);

        public async Task<(bool Success, string? Error)> UpdateOrderItemStatusAsync(string sellerId, int orderItemId, OrderStatus status)
        {
            var item = await _uow.Orders.GetOrderItemAsync(orderItemId);
            if (item is null || item.SellerId != sellerId)
                return (false, "Item not found or you do not have permission to modify it.");

            if (item.Status == OrderStatus.Cancelled)
                return (false, "Cannot update the status of a cancelled order.");

            item.Status = status;
            _uow.Orders.Update(item.Order!);

            // Keep the order-level status in sync: overall status = the "lowest" progress among items,
            // unless every item is delivered/cancelled.
            var order = item.Order!;
            // Reload with ALL items of the order (not just this seller's) to compute overall status.
            var fullOrder = await _uow.Orders.FirstOrDefaultAsync(o => o.Id == order.Id, includeProperties: "OrderItems");
            if (fullOrder != null)
            {
                if (fullOrder.OrderItems.All(i => i.Status == OrderStatus.Delivered))
                    fullOrder.Status = OrderStatus.Delivered;
                else if (fullOrder.OrderItems.All(i => i.Status == OrderStatus.Cancelled))
                    fullOrder.Status = OrderStatus.Cancelled;
                else if (fullOrder.OrderItems.Any(i => i.Status == OrderStatus.Shipped))
                    fullOrder.Status = OrderStatus.Shipped;
                else if (fullOrder.OrderItems.Any(i => i.Status == OrderStatus.Confirmed))
                    fullOrder.Status = OrderStatus.Confirmed;

                _uow.Orders.Update(fullOrder);
            }

            await _uow.CompleteAsync();
            return (true, null);
        }

        public async Task<SellerDashboardViewModel> GetDashboardAsync(string sellerId)
        {
            var products = await _uow.Products.GetBySellerAsync(sellerId);
            var items = await _uow.Orders.GetItemsBySellerAsync(sellerId);

            return new SellerDashboardViewModel
            {
                TotalProducts = products.Count(p => !p.IsDeletedBySeller),
                TotalOrders = items.Select(i => i.OrderId).Distinct().Count(),
                TotalSales = items.Where(i => i.Status == OrderStatus.Delivered).Sum(i => i.LineTotal)
            };
        }
    }
}
