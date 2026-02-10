using Microsoft.AspNetCore.Mvc;
using MovieExplorer.API.Core.Interfaces;

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
        public async Task<IActionResult> Search([FromQuery] string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest("Search query is required");
            }

            var movies = await _movieService.SearchMoviesAsync(query);
            return Ok(movies);
        }
    }
}


//Encapsulation
//-Controller exposes endpoint  //-Internal logic hidden inside service

//Dependency Injection
//-Controller depends on interface, not implementation

//Single Responsibility

//-Controller handles HTTP only   // - Service handles business logic