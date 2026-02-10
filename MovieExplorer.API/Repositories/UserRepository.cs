using Microsoft.EntityFrameworkCore;
using MovieExplorer.API.Data;
using MovieExplorer.API.Interfaces;
using MovieExplorer.API.Models;

namespace MovieExplorer.API.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByIdAsync(int userId)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.UserId == userId);
        }
    }
}
