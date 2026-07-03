using MovieExplorer.API.Application.DTOs;
using MovieExplorer.API.DTOs;

namespace MovieExplorer.API.Core.Interfaces
{
    public interface ILikeService
    {
        Task LikeMovieAsync(int userId, string movieId);
        Task<List<MovieDto>> GetLikedMoviesAsync(int userId);

        Task<PagedResponse<MovieDto>>
        GetLikedMoviesAsync(int userId, int pageNumber, int pageSize);

        Task UnlikeMovieAsync(int userId, string movieId);
    }
}
