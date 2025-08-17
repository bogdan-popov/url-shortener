using UrlShortener.Repositories.ShortUrlRep;
using UrlShortener.Repositories.UserRep;

namespace UrlShortener.Repositories.UnitOfWorkRep
{
    public interface IUnitOfWork
    {
        IUserRepository Users { get; }
        IShortUrlRepository ShortUrls { get; }
        Task<bool> CompleteAsync();
    }
}
