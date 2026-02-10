using System.ComponentModel.DataAnnotations;

namespace MovieExplorer.API.Models
{
    public class User
    {
        [Key]
        public int UserId { get; private set; }
        public string UserName { get; private set; }

        public User(int userId, string userName)
        {
            UserId = userId;
            UserName = userName;
        }
    }
}
