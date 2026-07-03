using MovieExplorer.API.Core.DTOs;

namespace MovieExplorer.API.Core.Interfaces
{
    public interface IOmdbService
    {
        Task<MovieDetailsDto?> GetMovieDetailsAsync(string imdbId);
    }
}