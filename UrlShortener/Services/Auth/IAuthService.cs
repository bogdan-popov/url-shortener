using UrlShortener.DTOs;

namespace UrlShortener.Services.Auth
{
    public interface IAuthService
    {
        Task<int> Register(UserRegisterDto request);
        Task<string> Login(UserRegisterDto request);
    }
}
