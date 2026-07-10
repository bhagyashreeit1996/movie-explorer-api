using MovieExplorer.API.Application.DTOs;
using MovieExplorer.API.Core.DTOs;
using MovieExplorer.API.DTOs;

namespace MovieExplorer.API.Core.Interfaces
{
    public interface IOmdbService
    {
        Task<MovieDetailsDto?> GetMovieDetailsAsync(string imdbId);

        Task<PagedResponse<MovieDto>> SearchMoviesAsync(SearchMoviesRequest request);
    }
}