using MovieExplorer.API.Models;

namespace MovieExplorer.API.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(int userId);
    }
}
