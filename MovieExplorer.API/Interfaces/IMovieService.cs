using MovieExplorer.API.DTOs;

namespace MovieExplorer.API.Interfaces
{
    public interface IMovieService
    {
        Task<List<MovieDto>> SearchMoviesAsync(string query);
    }
}
