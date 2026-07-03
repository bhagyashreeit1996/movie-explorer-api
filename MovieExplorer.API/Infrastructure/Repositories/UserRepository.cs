using Microsoft.EntityFrameworkCore;
using MovieExplorer.API.Core.Interfaces;
using MovieExplorer.API.Core.Models;
using MovieExplorer.API.Infrastructure.Data;

namespace MovieExplorer.API.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<User?> GetByIdAsync(int userId)
        {
            return await _context.Users
                .FirstOrDefaultAsync(x => x.UserId == userId);
        }

        public async Task AddAsync(User user)
        {
            _context.Users.Add(user);

            await _context.SaveChangesAsync();
        }
    }
}