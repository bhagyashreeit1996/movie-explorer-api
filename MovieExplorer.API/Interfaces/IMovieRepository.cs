using MovieExplorer.API.Models;

namespace MovieExplorer.API.Interfaces
{
    public interface IMovieRepository
    {
        Task<Movie?> GetByMovieIdAsync(string movieId);
        Task AddAsync(Movie movie);
        Task<bool> ExistsAsync(string movieId);
    }
}


//Abstraction

//Interface defines WHAT can be done

//Implementation defines HOW

///Dependency Inversion (SOLID)
//✅ Repository Interfaces