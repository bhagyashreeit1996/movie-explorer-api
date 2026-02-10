using System.ComponentModel.DataAnnotations;

namespace MovieExplorer.API.Models
{
    public class Movie
    {
        [Key]
        public string MovieId { get; private set; }   // IMDB ID
        public string Title { get; private set; }
        public int Year { get; private set; }
        public string Genre { get; private set; }

        // Constructor enforces valid object creation
        public Movie(string movieId, string title, int year, string genre)
        {
            MovieId = movieId;
            Title = title;
            Year = year;
            Genre = genre;
        }
    }
}

//here private setter means protected the data, properties are readable outside but we can't modified from outside.

//Encapsulation in this code is achieved by using private setters and a constructor,
//which protect the movie’s data and ensure it can only be created or modified in a controlled way.
