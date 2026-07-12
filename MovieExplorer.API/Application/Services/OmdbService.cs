using System.Text.Json;
using MovieExplorer.API.Application.DTOs;
using MovieExplorer.API.Core.DTOs;
using MovieExplorer.API.Core.Interfaces;
using MovieExplorer.API.DTOs;

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
            MovieDetailsDto? cachedMovie = null;

            try
            {
                //If Redis is unavailable, the app should continue by calling OMDb directly.
                cachedMovie = await _cacheService.GetAsync<MovieDetailsDto>(cacheKey);

                if (cachedMovie != null)
                {
                    _logger.LogInformation(
                        "Cache hit for movie {MovieId}",
                        imdbId);

                    return cachedMovie;
                }

                _logger.LogInformation(
                    "Cache miss for movie {MovieId}. Calling OMDb API.",
                    imdbId);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(
                    ex,
                    "Redis unavailable. Fetching movie from OMDb.");
            }

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
                try
                {
                    // here redis is optional
                    await _cacheService.SetAsync(
                        cacheKey,
                        movie,
                        TimeSpan.FromMinutes(30));

                    _logger.LogInformation(
                        "Movie {MovieId} cached successfully for 30 minutes.",
                        imdbId);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(
                        ex,
                        "Redis unavailable. Movie not cached.");
                }
            }

            return movie;

        }


        public async Task<PagedResponse<MovieDto>> SearchMoviesAsync(SearchMoviesRequest request)
        {

            var cacheKey =
                $"movie_search_{request.Query}_{request.PageNumber}";

            try
            {
                var cachedResult =
                    await _cacheService.GetAsync<PagedResponse<MovieDto>>(cacheKey);

                if (cachedResult != null)
                {
                    _logger.LogInformation(
                        "Redis HIT : {CacheKey}",
                        cacheKey);

                    return cachedResult;
                }

                _logger.LogInformation(
                    "Redis MISS : {CacheKey}",
                    cacheKey);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(
                    ex,
                    "Redis unavailable. Calling OMDb.");
            }

            var apiKey = _configuration["Omdb:ApiKey"];
            var baseUrl = _configuration["Omdb:BaseUrl"];

            var url =
                $"{baseUrl}?apikey={apiKey}&s={request.Query}&page={request.PageNumber}";

            var response = await _httpClient.GetAsync(url);

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<OmdbSearchResponse>(
                json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            if (result == null ||
                result.Response == "False" ||
                result.Search == null)
            {
                return new PagedResponse<MovieDto>
                {
                    Data = new List<MovieDto>(),
                    TotalCount = 0,
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize
                };
            }

            var movies = result.Search.Select(m => new MovieDto
            {
                MovieId = m.ImdbID,
                Title = m.Title,
                Year = int.TryParse(
                    m.Year.Split('-')[0],
                    out var year)
                        ? year
                        : 0,

                Genre = "N/A",

                Poster = m.Poster,

                ImdbRating = "N/A"

            }).ToList();

            var pagedResult = new PagedResponse<MovieDto>
            {
                Data = movies,
                TotalCount = int.Parse(result.TotalResults ?? "0"),
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };

            try
            {
                await _cacheService.SetAsync(
                    cacheKey,
                    pagedResult,
                    TimeSpan.FromMinutes(30));

                _logger.LogInformation(
                    "Redis SET : {CacheKey}",
                    cacheKey);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(
                    ex,
                    "Redis unavailable. Search result not cached.");
            }

            return pagedResult;
        }

    }
}