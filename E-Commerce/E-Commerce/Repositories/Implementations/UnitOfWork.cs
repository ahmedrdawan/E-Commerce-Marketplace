using E_Commerce.Models.Data;
using E_Commerce.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace E_Commerce.Repositories.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public IProductRepository Products { get; private set; }
        public IOrderRepository Orders { get; private set; }
        public ICartRepository CartItems { get; private set; }
        public IWishlistRepository WishlistItems { get; private set; }
        public ICategoryRepository Categories { get; private set; }
        public IReviewRepository Reviews { get; private set; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;

            Products = new ProductRepository(_context);
            Orders = new OrderRepository(_context);
            CartItems = new CartRepository(_context);
            WishlistItems = new WishlistRepository(_context);
            Categories = new CategoryRepository(_context);
            Reviews = new ReviewRepository(_context);
        }

        public async Task<int> CompleteAsync() => await _context.SaveChangesAsync();

        public async Task<IAppTransaction> BeginTransactionAsync()
        {
            var tx = await _context.Database.BeginTransactionAsync();
            return new AppTransaction(tx);
        }

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }

        private sealed class AppTransaction : IAppTransaction
        {
            private readonly IDbContextTransaction _tx;
            public AppTransaction(IDbContextTransaction tx)
            {
                _tx = tx;
            }

            public Task CommitAsync() => _tx.CommitAsync();
            public Task RollbackAsync() => _tx.RollbackAsync();
            public ValueTask DisposeAsync() => _tx.DisposeAsync();
        }
    }
}
