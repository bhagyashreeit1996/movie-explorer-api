using MovieExplorer.API.Application.DTOs;
using MovieExplorer.API.Core.Models;

namespace MovieExplorer.API.Core.Interfaces
{
    public interface IMovieLikeRepository
    {
        Task<bool> IsLikedAsync(int userId, string movieId);
        Task AddAsync(MovieLike like);

        Task<List<MovieLike>> GetUserLikesAsync(int userId);

        Task<(List<MovieDto> Movies, int TotalCount)>
        GetLikedMoviesAsync(int userId, int pageNumber, int pageSize);

        Task RemoveLikeAsync(int userId, string movieId);
    }
}