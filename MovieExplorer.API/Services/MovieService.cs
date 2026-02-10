using MovieExplorer.API.DTOs;
using MovieExplorer.API.Interfaces;
using MovieExplorer.API.Models;
using MovieExplorer.API.Services;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace MovieExplorer.API.Services
{
    public class MovieService : IMovieService
    {
        private readonly IMovieRepository _movieRepository;

        public MovieService(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }

        public async Task<List<MovieDto>> SearchMoviesAsync(string query)
        {
            // TEMP: Fake data for learning (external API comes later)
            var movie = new Movie("tt001", query, 2024, "Drama");

            if (!await _movieRepository.ExistsAsync(movie.MovieId))
            {
                await _movieRepository.AddAsync(movie);
            }

            return new List<MovieDto>
            {
                new MovieDto
                {
                    MovieId = movie.MovieId,
                    Title = movie.Title,
                    Year = movie.Year,
                    Genre = movie.Genre
                }
            };
        }
    }
}







//Composition
//-MovieService uses IMovieRepository
//-Not inheritance — composition is preferred

//Encapsulation
//-Controllers don’t know:  //-Where movies come from    //-How they are saved

//Dependency Injection
//-Repository injected via constructor
//-No new keyword for dependencies

//Single Responsibility Principle
//-MovieService = business logic only