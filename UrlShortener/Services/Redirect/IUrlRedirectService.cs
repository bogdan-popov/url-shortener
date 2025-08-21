namespace UrlShortener.Services.Redirect
{
    public interface IUrlRedirectService
    {
        Task<string?> GetOriginalUrlAsync(string shortCode);
    }
}
