using Microsoft.EntityFrameworkCore;
using MovieExplorer.API.Infrastructure.Data;
using MovieExplorer.API.Core.Interfaces;
using MovieExplorer.API.Core.Models;

namespace MovieExplorer.API.Infrastructure.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        private readonly ApplicationDbContext _context;

        public MovieRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Movie?> GetByMovieIdAsync(string movieId)
        {
            return await _context.Movies
                .FirstOrDefaultAsync(m => m.MovieId == movieId);
        }

        public async Task<List<Movie>> GetMoviesByIdsAsync(List<string> movieIds)
        {
            return await _context.Movies
                .Where(m => movieIds.Contains(m.MovieId))
                .ToListAsync();
        }

        public async Task<bool> ExistsAsync(string movieId)
        {
            return await _context.Movies
                .AnyAsync(m => m.MovieId == movieId);
        }

        public async Task AddAsync(Movie movie)
        {
            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();
        }

        public async Task<(List<Movie> Movies, int TotalCount)> SearchAsync(
        string query,
        int pageNumber,
        int pageSize,
        string? sortBy,
        bool isDescending)
        {
            var queryable = _context.Movies.AsQueryable();

            // Filtering
            queryable = queryable.Where(m => m.Title.Contains(query));

            // Sorting
            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                if (sortBy.Equals("Title", StringComparison.OrdinalIgnoreCase))
                    queryable = isDescending
                        ? queryable.OrderByDescending(m => m.Title)
                        : queryable.OrderBy(m => m.Title);

                if (sortBy.Equals("Year", StringComparison.OrdinalIgnoreCase))
                    queryable = isDescending
                        ? queryable.OrderByDescending(m => m.Year)
                        : queryable.OrderBy(m => m.Year);
            }

            var totalCount = await queryable.CountAsync();

            var movies = await queryable
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (movies, totalCount);
        }

        public async Task<List<Movie>> GetRecommendedMoviesAsync(int userId)
        {
            // Get all liked movie IDs for the user
            var likedMovieIds = await _context.MovieLikes
                .Where(x => x.UserId == userId)
                .Select(x => x.MovieId)
                .ToListAsync();

            // Get genres of liked movies
            var likedGenres = await _context.Movies
                .Where(x => likedMovieIds.Contains(x.MovieId))
                .Select(x => x.Genre)
                .ToListAsync();

            // Split genres into individual values
            var genreList = likedGenres
                .SelectMany(g => g.Split(',', StringSplitOptions.RemoveEmptyEntries))
                .Select(g => g.Trim())
                .Distinct()
                .ToList();

            // Recommend movies that share at least one genre
            var recommendations = await _context.Movies
                .Where(m =>
                    !likedMovieIds.Contains(m.MovieId) &&
                    genreList.Any(g => m.Genre.Contains(g)))
                .Take(5)
                .ToListAsync();

            return recommendations;
        }

        public async Task<List<string>> GetMovieSuggestionsAsync(string query)
        {
            return await _context.Movies
                .Where(m => m.Title.StartsWith(query))
                .Select(m => m.Title)
                .Distinct()
                .Take(5)
                .ToListAsync();
        }

    }
}
