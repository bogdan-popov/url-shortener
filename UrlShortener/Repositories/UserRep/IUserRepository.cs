using UrlShortener.Models;

namespace UrlShortener.Repositories.UserRep
{
    public interface IUserRepository
    {
        Task<User?> FindByUsernameAsync(string username);
        Task<User?> FindByIdAsync(int id);
        Task AddAsync(User user);
    }
}
