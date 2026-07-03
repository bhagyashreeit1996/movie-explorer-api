namespace MovieExplorer.API.Core.Models
{
    public class MovieLike
    {
        private MovieLike() { }

        public int MovieLikeId { get; private set; }
        public int UserId { get; private set; }
        public string MovieId { get; private set; }
        public DateTime LikedOn { get; private set; }

        public MovieLike(int userId, string movieId)
        {
            UserId = userId;
            MovieId = movieId;
            LikedOn = DateTime.UtcNow;
        }
    }
}