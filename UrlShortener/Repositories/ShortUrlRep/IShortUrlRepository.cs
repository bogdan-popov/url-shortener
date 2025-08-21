using UrlShortener.Models;

namespace UrlShortener.Repositories.ShortUrlRep
{
    public interface IShortUrlRepository
    {
        Task AddAsync(ShortUrl url);
        Task<ShortUrl?> FindByShortCodeAsync(string shortCode);
        Task<List<ShortUrl>> GetAllByUserIdAsync(int userId);
        void Delete(ShortUrl url);
    }
}
