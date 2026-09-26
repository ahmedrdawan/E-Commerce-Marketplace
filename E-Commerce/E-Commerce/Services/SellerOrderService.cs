using E_Commerce.Data.Repositories;
using E_Commerce.Entities;
using E_Commerce.Enums;

namespace E_Commerce.Services
{
    public class SellerOrderService : ISellerOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUnitOfWork _uow;

        public SellerOrderService(IOrderRepository orderRepository,IUnitOfWork uow)
        {
            _orderRepository = orderRepository;
            _uow = uow;
        }

        public async Task<IEnumerable<Order>> GetOrdersAsync(Guid sellerId)
        {
            return await _orderRepository.GetOrdersBySellerIdAsync(sellerId);
        }

        public async Task<Order?> GetOrderAsync(Guid orderId,Guid sellerId)
        {
            return await _orderRepository.GetOrderByIdForSellerAsync( orderId, sellerId);
        }

        public async Task UpdateStatusAsync( Guid orderId,Guid sellerId, OrderStatus status)
        {
            var order = await _orderRepository.GetOrderByIdForSellerAsync(orderId,sellerId);

            if (order == null)
                throw new KeyNotFoundException("Order not found.");

            if (status == OrderStatus.Cancelled)
                throw new InvalidOperationException("Seller cannot cancel an order.");

            if (order.Status == OrderStatus.Delivered)
                throw new InvalidOperationException("A delivered order cannot be updated.");

            order.Status = status;
            order.UpdatedAt = DateTime.UtcNow;

            await _orderRepository.UpdateAsync(order);

            await _uow.SaveChangesAsync();
        }
    }
}
