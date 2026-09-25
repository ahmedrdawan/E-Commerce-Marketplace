using System.Threading.Tasks;
using E_Commerce.Data.Repositories;

namespace E_Commerce.Data.Repositories
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync();
    }
}
