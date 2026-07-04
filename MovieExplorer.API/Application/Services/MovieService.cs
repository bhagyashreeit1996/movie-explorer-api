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

        public MovieService(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }

        public async Task<PagedResponse<MovieDto>> SearchMoviesAsync(SearchMoviesRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Query))
                throw new ArgumentException("Search query is required.");

            var (movies, totalCount) = await _movieRepository.SearchAsync(
                request.Query,
                request.PageNumber,
                request.PageSize,
                request.SortBy,
                request.IsDescending);

            var dtoList = movies.Select(m => new MovieDto
            {
                MovieId = m.MovieId,
                Title = m.Title,
                Year = m.Year,
                Genre = m.Genre,
                Poster = "https://via.placeholder.com/300x450?text=Movie",
                ImdbRating = "N/A"
            }).ToList();

            return new PagedResponse<MovieDto>
            {
                TotalCount = totalCount,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                Data = dtoList
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