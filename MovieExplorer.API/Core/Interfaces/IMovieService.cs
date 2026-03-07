using MovieExplorer.API.Application.DTOs;
using MovieExplorer.API.DTOs;

namespace MovieExplorer.API.Core.Interfaces
{
    public interface IMovieService
    {
        Task<PagedResponse<MovieDto>> SearchMoviesAsync(SearchMoviesRequest request);

    }
}
