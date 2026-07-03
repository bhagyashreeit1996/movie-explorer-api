using Microsoft.EntityFrameworkCore;
using MovieExplorer.API.Application.DTOs;
using MovieExplorer.API.Core.Interfaces;
using MovieExplorer.API.Core.Models;
using MovieExplorer.API.Infrastructure.Data;

namespace MovieExplorer.API.Infrastructure.Repositories
{
    public class MovieLikeRepository : IMovieLikeRepository
    {
        private readonly ApplicationDbContext _context;

        public MovieLikeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> IsLikedAsync(int userId, string movieId)
        {
            return await _context.MovieLikes
                .AnyAsync(l => l.UserId == userId && l.MovieId == movieId);
        }

        public async Task<(List<MovieDto> Movies, int TotalCount)>
        GetLikedMoviesAsync(int userId, int pageNumber, int pageSize)
        {
            var query =
                from like in _context.MovieLikes
                join movie in _context.Movies
                on like.MovieId equals movie.MovieId
                where like.UserId == userId
                select new MovieDto
                {
                    MovieId = movie.MovieId,
                    Title = movie.Title,
                    Year = movie.Year,
                    Genre = movie.Genre
                };

            var totalCount = await query.CountAsync();

            var movies = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (movies, totalCount);
        }

        public async Task<List<MovieLike>> GetUserLikesAsync(int userId)
        {
            return await _context.MovieLikes
                .Where(l => l.UserId == userId)
                .ToListAsync();
        }

        public async Task AddAsync(MovieLike like)
        {
            _context.MovieLikes.Add(like);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveLikeAsync(
        int userId,
        string movieId)
        {
            var like = await _context.MovieLikes
                .FirstOrDefaultAsync(x =>
                    x.UserId == userId &&
                    x.MovieId == movieId);

            if (like != null)
            {
                _context.MovieLikes.Remove(like);
                await _context.SaveChangesAsync();
            }
        }
    }
}
