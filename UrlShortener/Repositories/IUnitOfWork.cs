namespace UrlShortener.Repositories
{
    public interface IUnitOfWork
    {
        IUserRepository Users { get; }
        Task<bool> CompleteAsync();
    }
}
