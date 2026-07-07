using System.Text.Json;
using MovieExplorer.API.Core.DTOs;
using MovieExplorer.API.Core.Interfaces;

namespace MovieExplorer.API.Application.Services
{
    public class OmdbService : IOmdbService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ICacheService _cacheService;
        private readonly ILogger<OmdbService> _logger;

        public OmdbService(
            HttpClient httpClient,
            IConfiguration configuration,
            ICacheService cacheService,
            ILogger<OmdbService> logger)
                {
                    _httpClient = httpClient;
                    _configuration = configuration;
                    _cacheService = cacheService;
                    _logger = logger;
                }

        public async Task<MovieDetailsDto?> GetMovieDetailsAsync(string imdbId)
        {
            // Create a unique cache key
            var cacheKey = $"movie_details_{imdbId}";

            // Try to get movie details from Redis
            var cachedMovie = await _cacheService.GetAsync<MovieDetailsDto>(cacheKey);

            if(cachedMovie != null)
{
                _logger.LogInformation(
                    "Cache hit for movie {MovieId}",
                    imdbId);

                return cachedMovie;
            }

            _logger.LogInformation(
                "Cache miss for movie {MovieId}. Calling OMDb API.",
                imdbId);

            // If not found in Redis, call OMDb API
            var apiKey = _configuration["Omdb:ApiKey"];
            var baseUrl = _configuration["Omdb:BaseUrl"];

            var url = $"{baseUrl}?i={imdbId}&apikey={apiKey}";

            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var json = await response.Content.ReadAsStringAsync();

            var movie = JsonSerializer.Deserialize<MovieDetailsDto>(
                json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            // Save the response in Redis for 30 minutes
            if (movie != null)
            {
                await _cacheService.SetAsync(
                    cacheKey,
                    movie,
                    TimeSpan.FromMinutes(30));

                _logger.LogInformation(
                    "Movie {MovieId} cached successfully for 30 minutes.",
                    imdbId);
            }

            return movie;

        }
    
    }
}