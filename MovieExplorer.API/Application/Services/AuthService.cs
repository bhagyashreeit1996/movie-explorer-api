using MovieExplorer.API.Core.DTOs;
using MovieExplorer.API.Core.Interfaces;
using MovieExplorer.API.Core.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MovieExplorer.API.Exceptions;

namespace MovieExplorer.API.Core.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;

        public AuthService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        private readonly IConfiguration _configuration;

        public AuthService(
            IUserRepository userRepository,
            IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public async Task RegisterAsync(RegisterRequest request)
        {
            var existingUser =
                await _userRepository
                    .GetByEmailAsync(request.Email);

            if (existingUser != null)
                throw new UserAlreadyExistsException();

            var user = new User
            {
                Name = request.Name,
                Email = request.Email,

                // Temporary
                PasswordHash = request.Password
            };

            await _userRepository.AddAsync(user);
        }

        public async Task<AuthResponse> LoginAsync(
        LoginRequest request)
        {
            var user =
                await _userRepository
                    .GetByEmailAsync(request.Email);

            if (user == null)
                throw new InvalidCredentialsException();

            if (user.PasswordHash != request.Password)
                throw new InvalidCredentialsException();

            var token = GenerateJwtToken(user);

            return new AuthResponse
            {
                Email = user.Email,
                Token = token
            };
        }

        private string GenerateJwtToken(User user)
        {
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    _configuration["Jwt:Key"]));

            var credentials =
                new SigningCredentials(
                    key,
                    SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(
            ClaimTypes.NameIdentifier,
            user.UserId.ToString()),

            new Claim(
            ClaimTypes.Email,
            user.Email)
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler()
                .WriteToken(token);
        }

        public async Task<UserProfileDto?> GetCurrentUserAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);

            if (user == null)
                return null;

            return new UserProfileDto
            {
                UserId = user.UserId,
                Name = user.Name,
                Email = user.Email
            };
        }
    }
}