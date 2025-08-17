using UrlShortener.Data;

namespace UrlShortener.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _context;
        public IUserRepository Users { get; private set; }

        public UnitOfWork(DataContext context)
        {
            _context = context;
            Users = new UserRepository(_context);
        }

        public async Task<bool> CompleteAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
