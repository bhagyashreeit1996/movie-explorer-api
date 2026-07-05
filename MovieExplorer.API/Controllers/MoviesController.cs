using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieExplorer.API.Application.DTOs;
using MovieExplorer.API.Core.DTOs;
using MovieExplorer.API.Core.Exceptions;
using MovieExplorer.API.Core.Interfaces;
using MovieExplorer.API.DTOs;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace MovieExplorer.API.Controllers
{
    
    [ApiController]
    [Route("api/movies")]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieService _movieService;
        private readonly ILikeService _likeService;
        private readonly IOmdbService _omdbService;

        public MoviesController(IMovieService movieService, ILikeService likeService, IOmdbService omdbService)
        {
            _movieService = movieService;
            _likeService = likeService;
            _omdbService = omdbService;
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] SearchMoviesRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Query))
                return BadRequest("Search query is required.");

            if (request.PageNumber <= 0 || request.PageSize <= 0)
                return BadRequest("Invalid pagination parameters.");

            var result = await _movieService.SearchMoviesAsync(request);

            return Ok(result);
        }

        [Authorize]
        [HttpPost("{movieId}/like")]
        public async Task<IActionResult> LikeMovie(string movieId)
        {
            var userId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            await _likeService.LikeMovieAsync(userId, movieId);

            return Ok("Movie liked successfully.");
        }

        [HttpGet("/api/users/likes")]
        public async Task<IActionResult> GetLikedMovies(
        
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 5)
        {
            
            var claim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (claim == null)
            {
                return Unauthorized("User identifier claim is missing.");
            }
            var userId = int.Parse(claim.Value);

            var result = await _likeService
                .GetLikedMoviesAsync(userId, pageNumber, pageSize);

            return Ok(result);
        }

        [Authorize]
        [HttpDelete("{movieId}/like")]
        public async Task<IActionResult> UnlikeMovie(string movieId)
        {
            var userId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            await _likeService.UnlikeMovieAsync(userId, movieId);

            return Ok("Movie unliked successfully.");
        }

        
        [HttpGet("{movieId}/details")]
        public async Task<IActionResult> GetMovieDetails(string movieId)
        {
            var movie = await _omdbService.GetMovieDetailsAsync(movieId);

            if (movie == null)
            {
                return NotFound("Movie not found.");
            }

            return Ok(movie);
        }

        [Authorize]
        [HttpGet("recommendations")]
        public async Task<IActionResult> GetRecommendations()
        {
            var userId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var recommendations = await _movieService
                .GetRecommendedMoviesAsync(userId);

            return Ok(recommendations);
        }

        [HttpGet("suggestions")]
        public async Task<IActionResult> GetSuggestions([FromQuery] string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return Ok(new List<string>());

            var suggestions = await _movieService
                .GetMovieSuggestionsAsync(query);

            return Ok(suggestions);
        }

    }
}


//Encapsulation
//-Controller exposes endpoint  //-Internal logic hidden inside service

//Dependency Injection
//-Controller depends on interface, not implementation

//Single Responsibility

//-Controller handles HTTP only   // - Service handles business logic