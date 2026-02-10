using Microsoft.EntityFrameworkCore;
using MovieExplorer.API.Data;
using MovieExplorer.API.Models;

namespace MovieExplorer.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<MovieLike> MovieLikes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MovieLike>()
                .HasIndex(ml => new { ml.UserId, ml.MovieId })
                .IsUnique();

            base.OnModelCreating(modelBuilder);
        }
    }


}


//What Just Happened?

//ApplicationDbContext is a class
//It encapsulates database access
//Other layers never talk to DB directly
//This is Encapsulation + Single Responsibility