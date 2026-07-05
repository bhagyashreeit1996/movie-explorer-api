using MovieExplorer.API.Core.Models;

namespace MovieExplorer.API.Core.Interfaces
{
    public interface IMovieRepository
    {
        //Task GetByMovieIdAsync(string movieId);
        Task<Movie?> GetByMovieIdAsync(string movieId);
        Task<(List<Movie> Movies, int TotalCount)> SearchAsync(
        string query,
        int pageNumber,
        int pageSize,
        string? sortBy,
        bool isDescending);
        Task<List<Movie>> GetMoviesByIdsAsync(List<string> movieIds);

        Task<List<Movie>> GetRecommendedMoviesAsync(int userId);

        Task<List<string>> GetMovieSuggestionsAsync(string query);
    }
}


//Abstraction

//Interface defines WHAT can be done

//Implementation defines HOW

///Dependency Inversion (SOLID)
//✅ Repository Interfaces