using MovieExplorer.API.Application.DTOs;
using MovieExplorer.API.Core.Interfaces;
using MovieExplorer.API.Core.Models;
using MovieExplorer.API.Application.Services;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;
using Microsoft.EntityFrameworkCore;
using MovieExplorer.API.DTOs;

namespace MovieExplorer.API.Application.Services
{
    public class MovieService : IMovieService
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IOmdbService _omdbService;

        public MovieService(
            IMovieRepository movieRepository,
            IOmdbService omdbService)
                {
                    _movieRepository = movieRepository;
                    _omdbService = omdbService;
                }

        public async Task<PagedResponse<MovieDto>> SearchMoviesAsync(SearchMoviesRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Query))
                throw new ArgumentException("Search query is required.");

            return await _omdbService.SearchMoviesAsync(request);
        }

        public async Task<List<RecommendationDto>> GetRecommendedMoviesAsync(int userId)
        {
            var movies = await _movieRepository.GetRecommendedMoviesAsync(userId);

            return movies.Select(m => new RecommendationDto
            {
                MovieId = m.MovieId,
                Title = m.Title,
                Year = m.Year,
                Genre = m.Genre
            }).ToList();
        }

        public async Task<List<string>> GetMovieSuggestionsAsync(string query)
        {
            return await _movieRepository.GetMovieSuggestionsAsync(query);
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