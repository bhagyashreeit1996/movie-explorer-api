using MovieExplorer.API.Core.Models;

namespace MovieExplorer.API.Core.Interfaces
{
    public interface IMovieRepository
    {
        Task<(List<Movie> Movies, int TotalCount)> SearchAsync(
        string query,
        int pageNumber,
        int pageSize,
        string? sortBy,
        bool isDescending);
    }
}


//Abstraction

//Interface defines WHAT can be done

//Implementation defines HOW

///Dependency Inversion (SOLID)
//✅ Repository Interfaces