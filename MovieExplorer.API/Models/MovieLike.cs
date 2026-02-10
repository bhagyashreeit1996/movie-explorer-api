using System;
using System.ComponentModel.DataAnnotations;

namespace MovieExplorer.API.Models
{
    public class MovieLike
    {
        [Key]        
        
        public int LikeId { get; private set; }
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
