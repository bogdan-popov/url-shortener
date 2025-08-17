using UrlShortener.Data;
using UrlShortener.Repositories.ShortUrlRep;
using UrlShortener.Repositories.UserRep;

namespace UrlShortener.Repositories.UnitOfWorkRep
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _context;
        public IUserRepository Users { get; private set; }

        public IShortUrlRepository ShortUrls { get; private set; }

        public UnitOfWork(DataContext context)
        {
            _context = context;
            Users = new UserRepository(_context);
            ShortUrls = new ShortUrlRepository(_context);
        }

        public async Task<bool> CompleteAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
