using System.Text.Json.Serialization;

namespace MovieExplorer.API.Core.DTOs
{
    public class OmdbSearchResponse
    {
        [JsonPropertyName("Search")]
        public List<OmdbMovieDto>? Search { get; set; }

        [JsonPropertyName("totalResults")]
        public string? TotalResults { get; set; }

        [JsonPropertyName("Response")]
        public string? Response { get; set; }

        [JsonPropertyName("Error")]
        public string? Error { get; set; }
    }
}