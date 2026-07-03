using Microsoft.EntityFrameworkCore;
using MovieExplorer.API.Infrastructure.Data;
using MovieExplorer.API.Core.Models;

namespace MovieExplorer.API.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<MovieLike> MovieLikes { get; set; }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }


}


//What Just Happened?

//ApplicationDbContext is a class
//It encapsulates database access
//Other layers never talk to DB directly
//This is Encapsulation + Single Responsibility