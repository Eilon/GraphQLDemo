using AspNetCoreGraphQL.Server.DataModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreGraphQL.Server.DataAccess
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
