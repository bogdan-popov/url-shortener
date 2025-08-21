using StackExchange.Redis;
using UrlShortener.Repositories.UnitOfWorkRep;

namespace UrlShortener.Services.Redirect
{
    public class UrlRedirectService : IUrlRedirectService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDatabase _redisDatabase;

        public UrlRedirectService(IUnitOfWork unitOfWork, IConnectionMultiplexer redis)
        {
            _unitOfWork = unitOfWork;
            _redisDatabase = redis.GetDatabase();
        }

        public async Task<string?> GetOriginalUrlAsync(string shortCode)
        {
            var cacheUrl = await _redisDatabase.StringGetAsync(shortCode);

            if (!cacheUrl.IsNullOrEmpty) return cacheUrl.ToString();

            var shortUrl = await _unitOfWork.ShortUrls.FindByShortCodeAsync(shortCode);

            if (shortUrl == null) return null;

            await _redisDatabase.StringSetAsync(shortCode, shortUrl.OriginalUrl, TimeSpan.FromHours(1));

            return shortUrl.OriginalUrl;
        }
    }
}
