using Microsoft.EntityFrameworkCore;
using MovieExplorer.API.Data;
using MovieExplorer.API.Interfaces;
using MovieExplorer.API.Models;

namespace MovieExplorer.API.Repositories
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

        public async Task AddAsync(MovieLike like)
        {
            _context.MovieLikes.Add(like);
            await _context.SaveChangesAsync();
        }
    }
}


//Abstraction -	IMovieRepository
//Encapsulation - EF Core hidden inside repo
//Dependency Injection -	DbContext via constructor
//SRP - Repo only handles data access