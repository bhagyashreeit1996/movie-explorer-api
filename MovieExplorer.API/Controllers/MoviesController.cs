using Microsoft.AspNetCore.Mvc;
using MovieExplorer.API.Core.Interfaces;
using MovieExplorer.API.DTOs;

namespace MovieExplorer.API.Controllers
{
    [ApiController]
    [Route("api/movies")]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieService _movieService;

        public MoviesController(IMovieService movieService)
        {
            _movieService = movieService;
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
    }
}


//Encapsulation
//-Controller exposes endpoint  //-Internal logic hidden inside service

//Dependency Injection
//-Controller depends on interface, not implementation

//Single Responsibility

//-Controller handles HTTP only   // - Service handles business logic