using E_Commerce.Entities;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Data.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly EcommerceDbContext _db;

        public OrderRepository(EcommerceDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Order>> GetOrdersBySellerIdAsync(
            Guid sellerId)
        {
            return await _db.Orders
                .Include(o => o.Customer)
                .Include(o => o.Items)
                    .ThenInclude(i => i.Product)
                .Where(o =>
                    o.Items.Any(i => i.SellerId == sellerId))
                .OrderByDescending(o => o.CreatedAt)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Order?> GetOrderByIdForSellerAsync(
            Guid orderId,
            Guid sellerId)
        {
            return await _db.Orders
                .Include(o => o.Customer)
                .Include(o => o.Items)
                    .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(o =>
                    o.Id == orderId &&
                    o.Items.Any(i => i.SellerId == sellerId));
        }

        public async Task UpdateAsync(Order order)
        {
            _db.Orders.Update(order);
        }
    }
}
