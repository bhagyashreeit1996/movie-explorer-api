using MovieExplorer.API.Models;

namespace MovieExplorer.API.Interfaces
{
    public interface IMovieLikeRepository
    {
        Task<bool> IsLikedAsync(int userId, string movieId);
        Task AddAsync(MovieLike like);
    }
}
