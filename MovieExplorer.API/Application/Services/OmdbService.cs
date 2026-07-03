using System.Text.Json;
using MovieExplorer.API.Core.DTOs;
using MovieExplorer.API.Core.Interfaces;

namespace MovieExplorer.API.Application.Services
{
    public class OmdbService : IOmdbService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public OmdbService(
            HttpClient httpClient,
            IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<MovieDetailsDto?> GetMovieDetailsAsync(string imdbId)
        {
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

            return movie;
        }
    }
}