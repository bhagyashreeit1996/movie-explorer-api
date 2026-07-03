using Microsoft.EntityFrameworkCore;
using MovieExplorer.API.Infrastructure.Data;
using MovieExplorer.API.Core.Interfaces;
using MovieExplorer.API.Infrastructure.Repositories;
using MovieExplorer.API.Application.Services;
using MovieExplorer.API.Core.Services;

namespace MovieExplorer.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Repository registrations
            services.AddScoped<IMovieRepository, MovieRepository>();

            services.AddScoped<IMovieLikeRepository, MovieLikeRepository>();

            services.AddScoped<IUserRepository, UserRepository>();

            // Service registrations
            services.AddScoped<IMovieService, MovieService>();

            services.AddScoped<ILikeService, LikeService>();

            services.AddScoped<IAuthService, AuthService>();

            return services;
        }

        public static IServiceCollection AddDatabaseServices(this IServiceCollection services, IConfiguration config)
        {
            var connectionString = config.GetConnectionString("DefaultConnection");

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            return services;
        }
    }
}
