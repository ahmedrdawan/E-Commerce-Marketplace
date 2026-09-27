using System.Linq.Expressions;

namespace E_Commerce.Repositories.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(int id);
        Task<IReadOnlyList<T>> GetAllAsync();

        Task<IReadOnlyList<T>> FindAsync(
            Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            string? includeProperties = null,
            int? skip = null,
            int? take = null);

        Task<int> CountAsync(Expression<Func<T, bool>>? filter = null);
        Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> filter, string? includeProperties = null);

        Task AddAsync(T entity);
        void Update(T entity);
        void Remove(T entity);
    }
}
