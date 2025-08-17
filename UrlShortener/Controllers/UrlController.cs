using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UrlShortener.DTOs;
using UrlShortener.Models;
using UrlShortener.Services.UrlShortening;

namespace UrlShortener.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UrlController : ControllerBase
    {
        private readonly IUrlShorteningService _urlShorteningService;

        public UrlController(IUrlShorteningService urlShorteningService)
        {
            _urlShorteningService = urlShorteningService;
        }

        [Authorize]
        [HttpPost("shorten")]
        public async Task<IActionResult> CreateShortUrl(CreateShortUrlDto createUrlDto)
        {
            int userId = GetCurrentUserId();
            ShortUrl newUrl = await _urlShorteningService.CreateShortUrlAsync(createUrlDto.OriginalUrl, userId);

            return Ok(new ShortUrlDto
            {
                OriginalUrl = createUrlDto.OriginalUrl,
                ShortUrl = newUrl.ShortCode,
            });
        }

        private int GetCurrentUserId()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
            {
                throw new Exception("Claim NameIdentifier не найден в токене.");
            }

            return Convert.ToInt32(userIdClaim.Value);
        }
    }
}
