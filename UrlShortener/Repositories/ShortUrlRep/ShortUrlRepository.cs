using Microsoft.EntityFrameworkCore;
using UrlShortener.Data;
using UrlShortener.Models;

namespace UrlShortener.Repositories.ShortUrlRep
{
    public class ShortUrlRepository : IShortUrlRepository
    {
        private readonly DataContext _context;

        public ShortUrlRepository(DataContext context)
        {
            _context = context;
        }

        public async Task AddAsync(ShortUrl url)
        {
            await _context.ShortUrls.AddAsync(url);
        }

        public async Task<ShortUrl?> FindByShortCodeAsync(string shortCode)
        {
            return await _context.ShortUrls.FirstOrDefaultAsync(s => s.ShortCode == shortCode);
        }
    }
}
