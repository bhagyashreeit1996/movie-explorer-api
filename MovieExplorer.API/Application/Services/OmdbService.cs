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

        public OmdbService(
            HttpClient httpClient,
            IConfiguration configuration,
            ICacheService cacheService)
                {
                    _httpClient = httpClient;
                    _configuration = configuration;
                    _cacheService = cacheService;
                }

        public async Task<MovieDetailsDto?> GetMovieDetailsAsync(string imdbId)
        {
            // Create a unique cache key
            var cacheKey = $"movie_details_{imdbId}";

            // Try to get movie details from Redis
            var cachedMovie = await _cacheService.GetAsync<MovieDetailsDto>(cacheKey);

            if (cachedMovie != null)
            {
                Console.WriteLine("******** Loaded from Redis Cache ********");
                return cachedMovie;
            }

            Console.WriteLine("******** Loaded from OMDb API ********");

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
            }

            return movie;

        }
    
    }
}