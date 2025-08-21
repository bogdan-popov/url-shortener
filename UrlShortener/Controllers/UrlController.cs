using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UrlShortener.DTOs;
using UrlShortener.Models;
using UrlShortener.Services.Url;

namespace UrlShortener.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UrlController : ControllerBase
    {
        private readonly IUrlService _urlService;
        private readonly IMapper _mapper;

        public UrlController(IUrlService urlShorteningService, IMapper mapper)
        {
            _urlService = urlShorteningService;
            _mapper = mapper;
        }

        [Authorize]
        [HttpPost("shorten")]
        public async Task<IActionResult> CreateShortUrl(CreateShortUrlDto createUrlDto)
        {
            int userId = GetCurrentUserId();
            ShortUrl newUrl = await _urlService.CreateShortUrlAsync(createUrlDto.OriginalUrl, userId);

            return Ok(new ShortUrlDto
            {
                OriginalUrl = createUrlDto.OriginalUrl,
                ShortCode = newUrl.ShortCode,
            });
        }

        [HttpGet("my-urls"), Authorize]
        public async Task<IActionResult> GetMyUrls()
        {
            var userId = GetCurrentUserId();
            var urls = await _urlService.GetAllUrlsForUserAsync(userId);

            var result = _mapper.Map<List<ShortUrlDto>>(urls);

            return Ok(result);
        }

        [HttpDelete("{shortCode}"), Authorize]
        public async Task<IActionResult> DeleteUrl(string shortCode)
        {
            var userId = GetCurrentUserId();

            await _urlService.DeleteUrlAsync(shortCode, userId);

            return NoContent();
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
