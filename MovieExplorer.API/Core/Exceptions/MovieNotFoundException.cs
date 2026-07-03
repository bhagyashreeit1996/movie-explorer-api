namespace MovieExplorer.API.Core.Exceptions
{
    public class MovieNotFoundException : Exception
    {
        public MovieNotFoundException(string movieId)
            : base($"Movie with id '{movieId}' was not found.")
        {
        }
    }

}
