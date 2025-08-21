using UrlShortener.Exceptions;
using UrlShortener.Models;
using UrlShortener.Repositories.UnitOfWorkRep;

namespace UrlShortener.Services.Url
{
    public class UrlService : IUrlService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UrlService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ShortUrl> CreateShortUrlAsync(string originalUrl, int userId)
        {
            var user = await _unitOfWork.Users.FindByIdAsync(userId);
            if (user == null) throw new Exception("Пользователь не найден.");

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

        public async Task DeleteUrlAsync(string shortCode, int userId)
        {
            var urlToDelete = await _unitOfWork.ShortUrls.FindByShortCodeAsync(shortCode);

            if (urlToDelete == null) throw new NotFoundException($"Ссылка с кодом '{shortCode}' не найдена.");
            if (urlToDelete.CreatedById != userId) throw new Exception("У вас нет прав на удаление этой ссылки.");

            _unitOfWork.ShortUrls.Delete(urlToDelete);
            await _unitOfWork.CompleteAsync();
        }

        public Task<List<ShortUrl>> GetAllUrlsForUserAsync(int userId)
        {
            return _unitOfWork.ShortUrls.GetAllByUserIdAsync(userId);
        }
    }
}
