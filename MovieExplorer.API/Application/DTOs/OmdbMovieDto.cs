using System.Text.Json.Serialization;

namespace MovieExplorer.API.Core.DTOs
{
    public class OmdbMovieDto
    {
        [JsonPropertyName("imdbID")]
        public string ImdbID { get; set; }

        [JsonPropertyName("Title")]
        public string Title { get; set; }

        [JsonPropertyName("Year")]
        public string Year { get; set; }

        [JsonPropertyName("Poster")]
        public string Poster { get; set; }
    }
}