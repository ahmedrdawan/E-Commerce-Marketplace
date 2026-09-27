using E_Commerce.Enums;
using E_Commerce.Models.Entities;
using E_Commerce.Repositories.Interfaces;
using E_Commerce.Services.Interfaces;
using E_Commerce.ViewModels.Order;

namespace E_Commerce.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _uow;

        public OrderService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<(bool Success, string? Error, int? OrderId)> PlaceOrderAsync(string customerId, CheckoutViewModel viewModel)
        {
            var cartItems = await _uow.CartItems.GetCartAsync(customerId);
            if (cartItems.Count == 0)
                return (false, "Your cart is empty.", null);

            foreach (var item in cartItems)
            {
                if (item.Product is null || item.Product.IsRemovedByAdmin || item.Product.IsDeletedBySeller)
                    return (false, $"Product \"{item.Product?.Name}\" is no longer available.", null);

                if (item.Quantity > item.Product.AvailableQuantity)
                    return (false, $"The available quantity of \"{item.Product.Name}\" is only {item.Product.AvailableQuantity}.", null);
            }

            await using var transaction = await _uow.BeginTransactionAsync();
            try
            {
                var order = new Order
                {
                    CustomerId = customerId,
                    ShippingAddress = viewModel.ShippingAddress,
                    OrderDate = DateTime.UtcNow,
                    Status = OrderStatus.Pending
                };

                foreach (var item in cartItems)
                {
                    var product = item.Product!;
                    order.OrderItems.Add(new OrderItem
                    {
                        ProductId = product.Id,
                        SellerId = product.SellerId,
                        Quantity = item.Quantity,
                        UnitPrice = product.Price,
                        Status = OrderStatus.Pending
                    });

                    product.AvailableQuantity -= item.Quantity;
                    _uow.Products.Update(product);
                }

                order.TotalPrice = order.OrderItems.Sum(i => i.LineTotal);

                await _uow.Orders.AddAsync(order);

                foreach (var item in cartItems)
                    _uow.CartItems.Remove(item);

                await _uow.CompleteAsync();
                await transaction.CommitAsync();

                return (true, null, order.Id);
            }
            catch
            {
                await transaction.RollbackAsync();
                return (false, "An error occurred while creating the order.", null);
            }
        }

        public async Task<IReadOnlyList<OrderViewModel>> GetMyOrdersAsync(string customerId)
        {
            var orders = await _uow.Orders.GetByCustomerAsync(customerId);
            return orders.Select(MapToViewModel).ToList();
        }

        public async Task<OrderViewModel?> GetOrderDetailsAsync(string customerId, int orderId)
        {
            var order = await _uow.Orders.FirstOrDefaultAsync(o => o.Id == orderId && o.CustomerId == customerId,
                includeProperties: "OrderItems,OrderItems.Product,OrderItems.Seller");
            return order is null ? null : MapToViewModel(order);
        }

        public async Task<(bool Success, string? Error)> CancelOrderAsync(string customerId, int orderId)
        {
            var order = await _uow.Orders.FirstOrDefaultAsync(o => o.Id == orderId && o.CustomerId == customerId,
                includeProperties: "OrderItems,OrderItems.Product");

            if (order is null)
                return (false, "Order not found.");

            if (order.Status is OrderStatus.Shipped or OrderStatus.Delivered or OrderStatus.Cancelled)
                return (false, "The order cannot be cancelled at this stage.");

            order.Status = OrderStatus.Cancelled;
            foreach (var item in order.OrderItems)
            {
                item.Status = OrderStatus.Cancelled;
                if (item.Product != null)
                {
                    item.Product.AvailableQuantity += item.Quantity;
                    _uow.Products.Update(item.Product);
                }
            }

            _uow.Orders.Update(order);
            await _uow.CompleteAsync();
            return (true, null);
        }

        public async Task<IReadOnlyList<Order>> GetAllOrdersAsync() =>
            await _uow.Orders.FindAsync(orderBy: q => q.OrderByDescending(o => o.OrderDate),
                includeProperties: "Customer,OrderItems");

        public async Task<Order?> GetOrderForAdminAsync(int orderId) =>
            await _uow.Orders.FirstOrDefaultAsync(o => o.Id == orderId,
                includeProperties: "Customer,OrderItems,OrderItems.Product,OrderItems.Seller");

        private static OrderViewModel MapToViewModel(Order order) => new()
        {
            Id = order.Id,
            OrderDate = order.OrderDate,
            TotalPrice = order.TotalPrice,
            Status = order.Status,
            ShippingAddress = order.ShippingAddress,
            Items = order.OrderItems.Select(i => new OrderItemViewModel
            {
                Id = i.Id,
                ProductName = i.Product?.Name ?? string.Empty,
                SellerName = i.Seller?.FullName,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice,
                Status = i.Status
            }).ToList()
        };
    }
}
