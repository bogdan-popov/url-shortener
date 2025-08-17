using UrlShortener.Models;

namespace UrlShortener.Repositories.ShortUrlRep
{
    public interface IShortUrlRepository
    {
        Task AddAsync(ShortUrl url);
        Task<ShortUrl?> FindByShortCodeAsync(string shortCode);
    }
}
