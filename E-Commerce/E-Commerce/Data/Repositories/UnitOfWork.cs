namespace E_Commerce.Data.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly EcommerceDbContext _db;

        public UnitOfWork(EcommerceDbContext db)
        {
            _db = db;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _db.SaveChangesAsync();
        }
    }
}
