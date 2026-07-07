namespace MovieExplorer.API.Exceptions
{
    public class UserAlreadyExistsException : Exception
    {
        public UserAlreadyExistsException()
            : base("User already exists.")
        {
        }

        public UserAlreadyExistsException(string message)
            : base(message)
        {
        }
    }
}