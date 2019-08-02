using Microsoft.EntityFrameworkCore;
using MovieDbSample.DataModels;

namespace MovieDbSample.Server.DataAccess
{
    public class MoviesDbContext : DbContext
    {
        public MoviesDbContext(DbContextOptions<MoviesDbContext> options)
            : base(options)
        {
        }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<CastMember> CastMembers { get; set; }
        public DbSet<Actor> Actors { get; set; }
    }
}
