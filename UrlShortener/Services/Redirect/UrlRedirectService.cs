using StackExchange.Redis;
using UrlShortener.Repositories.UnitOfWorkRep;

namespace UrlShortener.Services.Redirect
{
    public class UrlRedirectService : IUrlRedirectService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDatabase _redisDatabase;
        private readonly ILogger<UrlRedirectService> _logger;

        public UrlRedirectService(IUnitOfWork unitOfWork, IConnectionMultiplexer redis, ILogger<UrlRedirectService> logger)
        {
            _unitOfWork = unitOfWork;
            _redisDatabase = redis.GetDatabase();
            _logger = logger;
        }

        public async Task<string?> GetOriginalUrlAsync(string shortCode)
        {
            var cachedUrl = await _redisDatabase.StringGetAsync(shortCode);

            if (!cachedUrl.IsNullOrEmpty)
            {
                _logger.LogInformation("Cache HIT for short code: {ShortCode}", shortCode);
                return cachedUrl.ToString();
            }

            _logger.LogInformation("Cache MISS for short code: {ShortCode}. Fetching from database.", shortCode);

            var shortUrl = await _unitOfWork.ShortUrls.FindByShortCodeAsync(shortCode);

            if (shortUrl == null) return null;

            await _redisDatabase.StringSetAsync(shortCode, shortUrl.OriginalUrl, TimeSpan.FromHours(1));

            return shortUrl.OriginalUrl;
        }
    }
}
