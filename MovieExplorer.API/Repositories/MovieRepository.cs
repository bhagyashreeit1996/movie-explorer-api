using Microsoft.EntityFrameworkCore;
using MovieExplorer.API.Data;
using MovieExplorer.API.Interfaces;
using MovieExplorer.API.Models;

namespace MovieExplorer.API.Repositories
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
    }
}
