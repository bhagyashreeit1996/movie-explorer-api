using MovieExplorer.API.Application.DTOs;

namespace MovieExplorer.API.Core.Interfaces
{
    public interface IMovieService
    {
        Task<List<MovieDto>> SearchMoviesAsync(string query);
    }
}
