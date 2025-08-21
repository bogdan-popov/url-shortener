using UrlShortener.Models;

namespace UrlShortener.Services.Url
{
    public interface IUrlService
    {
        Task<ShortUrl> CreateShortUrlAsync(string originalUrl, int userId);
        Task<List<ShortUrl>> GetAllUrlsForUserAsync(int userId);
        Task DeleteUrlAsync(string shortCode, int userId);
    }
}
