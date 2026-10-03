namespace E_Commerce.Repositories.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IProductRepository Products { get; }
        IOrderRepository Orders { get; }
        ICartRepository CartItems { get; }
        IWishlistRepository WishlistItems { get; }
        ICategoryRepository Categories { get; }
        IReviewRepository Reviews { get; }

        Task<int> CompleteAsync();
        Task<IAppTransaction> BeginTransactionAsync();
    }

    public interface IAppTransaction : IAsyncDisposable
    {
        Task CommitAsync();
        Task RollbackAsync();
    }
}
