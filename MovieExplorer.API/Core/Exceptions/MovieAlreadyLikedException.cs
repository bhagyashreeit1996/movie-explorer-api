namespace MovieExplorer.API.Core.Exceptions
{
    public class MovieAlreadyLikedException : Exception
    {
        public MovieAlreadyLikedException(string movieId)
            : base($"Movie '{movieId}' is already liked.")
        {
        }
    }
}
