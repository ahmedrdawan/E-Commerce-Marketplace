using E_Commerce.Models.Data;
using E_Commerce.Models.Entities;
using E_Commerce.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Repositories.Implementations
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IReadOnlyList<Order>> GetByCustomerAsync(string customerId) =>
            await _dbSet.Include(o => o.OrderItems).ThenInclude(oi => oi.Product)
                        .Where(o => o.CustomerId == customerId)
                        .OrderByDescending(o => o.OrderDate)
                        .ToListAsync();

        public async Task<IReadOnlyList<OrderItem>> GetItemsBySellerAsync(string sellerId) =>
            await _context.OrderItems
                        .Include(oi => oi.Product)
                        .Include(oi => oi.Order).ThenInclude(o => o!.Customer)
                        .Where(oi => oi.SellerId == sellerId)
                        .OrderByDescending(oi => oi.Order!.OrderDate)
                        .ToListAsync();

        public async Task<OrderItem?> GetOrderItemAsync(int orderItemId) =>
            await _context.OrderItems
                        .Include(oi => oi.Order)
                        .Include(oi => oi.Product)
                        .FirstOrDefaultAsync(oi => oi.Id == orderItemId);
    }
}
