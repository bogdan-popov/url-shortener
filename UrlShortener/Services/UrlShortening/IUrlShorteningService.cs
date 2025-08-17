using UrlShortener.Models;

namespace UrlShortener.Services.UrlShortening
{
    public interface IUrlShorteningService
    {
        Task<ShortUrl> CreateShortUrlAsync(string originalUrl, int userId);
    }
}
