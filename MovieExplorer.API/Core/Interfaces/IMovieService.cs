using MovieExplorer.API.Application.DTOs;
using MovieExplorer.API.DTOs;

namespace MovieExplorer.API.Core.Interfaces
{
    public interface IMovieService
    {
        Task<PagedResponse<MovieDto>> SearchMoviesAsync(SearchMoviesRequest request);

        Task<List<RecommendationDto>> GetRecommendedMoviesAsync(int userId);

        Task<List<string>> GetMovieSuggestionsAsync(string query);

    }
}
