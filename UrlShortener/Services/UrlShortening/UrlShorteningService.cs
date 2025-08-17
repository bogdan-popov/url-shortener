using UrlShortener.Models;
using UrlShortener.Repositories.UnitOfWorkRep;
using Microsoft.EntityFrameworkCore;
namespace UrlShortener.Services.UrlShortening
{
    public class UrlShorteningService : IUrlShorteningService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UrlShorteningService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ShortUrl> CreateShortUrlAsync(string originalUrl, int userId)
        {
            var user = await _unitOfWork.Users.FindByIdAsync(userId);
            if (user == null)
                throw new Exception("Пользователь не найден.");

            string newCode;
            ShortUrl? existingUrl;

            do
            {
                newCode = Guid.NewGuid().ToString().Substring(0, 8);
                existingUrl = await _unitOfWork.ShortUrls.FindByShortCodeAsync(newCode);
            } while (existingUrl != null);

            var newShortUrl = new ShortUrl
            {
                OriginalUrl = originalUrl,
                ShortCode = newCode,
                CreatedAt = DateTime.UtcNow,
                CreatedById = userId,
                User = user
            };

            await _unitOfWork.ShortUrls.AddAsync(newShortUrl);
            await _unitOfWork.CompleteAsync();

            return newShortUrl;
        }
    }
}
