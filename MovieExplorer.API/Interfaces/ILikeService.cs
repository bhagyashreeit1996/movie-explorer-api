using Microsoft.AspNetCore.Mvc;

namespace MovieExplorer.API.Interfaces
{
    public interface ILikeService
    {
        Task LikeMovieAsync(int userId, string movieId);
    }
}




//Abstraction

//-Interfaces define what business operations exist
//-Controllers depend on interfaces, not implementations

//Dependency Inversion Principle
//-Service interfaces

//Encapsulation
//-Business rules hidden inside service implementations
//-Controllers don’t know the “how”