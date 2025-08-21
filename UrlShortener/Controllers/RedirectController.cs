using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UrlShortener.Repositories.UnitOfWorkRep;
using UrlShortener.Services.Redirect;

namespace UrlShortener.Controllers
{
    [Route("{shortCode}")]
    [ApiController]
    public class RedirectController : ControllerBase
    {
        private readonly IUrlRedirectService _redirectService;

        public RedirectController(IUrlRedirectService redirectService)
        {
            _redirectService = redirectService;
        }

        [HttpGet]
        public async Task<IActionResult> RedirectToUrl(string shortCode)
        {
            var originalUrl = await _redirectService.GetOriginalUrlAsync(shortCode);

            if (originalUrl == null) return NotFound();

            return Redirect(originalUrl);
        }
    }
}
