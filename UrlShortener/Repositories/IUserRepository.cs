using UrlShortener.Models;

namespace UrlShortener.Repositories
{
    public interface IUserRepository
    {
        Task<User?> FindByUsernameAsync(string username);
        Task AddAsync(User user);
    }
}
