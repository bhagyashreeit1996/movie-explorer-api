using MovieExplorer.API.Core.DTOs;

namespace MovieExplorer.API.Core.Interfaces
{
    public interface IAuthService
    {
        Task RegisterAsync(RegisterRequest request);

        Task<AuthResponse> LoginAsync(LoginRequest request);

        Task<UserProfileDto?> GetCurrentUserAsync(int userId);
    }

}