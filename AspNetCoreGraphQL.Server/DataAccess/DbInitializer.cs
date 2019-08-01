using AspNetCoreGraphQL.Server.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreGraphQL.Server.DataAccess
{
    public static class DbInitializer
    {
        public static void Initialize(MoviesDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.Movies.Any())
            {
                // If there's anything in the database, assume it is fully seeded
                return;
            }

            // -------------------- ACTORS --------------------
            var keanuActor = new Actor { Name = "Keanu Reeves", DOB = new DateTime(1964, 09, 02), };
            var larryActor = new Actor { Name = "Laurence Fishburne", DOB = new DateTime(1961, 07, 30), };
            var halleActor = new Actor { Name = "Halle Berry", DOB = new DateTime(1966, 08, 14), };
            var scarlettActor = new Actor { Name = "Scarlett Johansson", DOB = new DateTime(1984, 11, 22), };
            var choiActor = new Actor { Name = "Choi Min-sik", DOB = new DateTime(1962, 05, 30), };
            var robertActor = new Actor { Name = "Robert Downey Jr.", DOB = new DateTime(1965, 04, 04), };
            var gwynethActor = new Actor { Name = "Gwyneth Paltrow", DOB = new DateTime(1972, 09, 27), };
            var samActor = new Actor { Name = "Samuel L. Jackson", DOB = new DateTime(1948, 12, 21), };
            var actors = new[]
            {
                keanuActor,
                larryActor,
                halleActor,
                scarlettActor,
                choiActor,
                robertActor,
                gwynethActor,
                samActor,
            };
            foreach (var a in actors)
            {
                context.Actors.Add(a);
            }
            context.SaveChanges();

            // -------------------- CAST MEMBERS --------------------
            var neoCast = new CastMember { Actor = keanuActor, Character = "Neo", OrderOfAppearance = 1, };
            var morphCast = new CastMember { Actor = larryActor, Character = "Morpheus", OrderOfAppearance = 2, };

            var wickCast = new CastMember { Actor = keanuActor, Character = "John Wick", OrderOfAppearance = 1, };
            var boweryCast = new CastMember { Actor = larryActor, Character = "Bowery King", OrderOfAppearance = 2, };
            var sofiaCast = new CastMember { Actor = halleActor, Character = "Sophia", OrderOfAppearance = 3, };

            var lucyCast = new CastMember { Actor = scarlettActor, Character = "Lucy Miller", OrderOfAppearance = 1, };
            var jangCast = new CastMember { Actor = choiActor, Character = "Mr. Jang", OrderOfAppearance = 2, };

            var yiCast = new CastMember { Actor = choiActor, Character = "Yi Sun-sin", OrderOfAppearance = 1, };

            var natashaCast = new CastMember { Actor = scarlettActor, Character = "Natasha Romanoff (Black Widow)", OrderOfAppearance = 2, };
            var tonyCast = new CastMember { Actor = robertActor, Character = "Tony Stark", OrderOfAppearance = 1, };
            var pepperCast = new CastMember { Actor = gwynethActor, Character = "Pepper Potts", OrderOfAppearance = 3, };
            var nickCast = new CastMember { Actor = samActor, Character = "Nick Fury", OrderOfAppearance = 4, };

            var russCast = new CastMember { Actor = samActor, Character = "Russell Franklin", OrderOfAppearance = 3, };

            var castMembers = new[]
            {
                neoCast,
                morphCast,

                wickCast,
                boweryCast,
                sofiaCast,

                lucyCast,
                jangCast,

                yiCast,

                natashaCast,
                tonyCast,
                pepperCast,
                nickCast,

                russCast,
            };
            foreach (var c in castMembers)
            {
                context.CastMembers.Add(c);
            }
            context.SaveChanges();

            // -------------------- MOVIES --------------------
            var movies = new[]
            {
                new Movie {
                    Title = "The Matrix",
                    Year = 1999,
                    Language = "English",
                    Genre = Genre.Action,
                    Cast = new List<CastMember>{ neoCast, morphCast },
                },
                new Movie {
                    Title = "John Wick: Chapter 3 – Parabellum",
                    Year = 2019,
                    Language = "English",
                    Genre = Genre.Action,
                    Cast = new List<CastMember>{ wickCast, boweryCast, sofiaCast, },
                },
                new Movie {
                    Title = "Lucy",
                    Year = 2014,
                    Language = "English",
                    Genre = Genre.SciFiFantasy,
                    Cast = new List<CastMember>{ lucyCast, jangCast, },
                },
                new Movie {
                    Title = "명량 (The Admiral: Roaring Currents)",
                    Year = 2014,
                    Language = "Korean",
                    Genre = Genre.Action,
                    Cast = new List<CastMember>{ yiCast, },
                },
                new Movie {
                    Title = "Iron Man 2",
                    Year = 2010,
                    Language = "English",
                    Genre = Genre.Action,
                    Cast = new List<CastMember>{ natashaCast, tonyCast, pepperCast, nickCast, },
                },
                new Movie {
                    Title = "Deep Blue Sea",
                    Year = 1999,
                    Language = "English",
                    Genre = Genre.Horror,
                    Cast = new List<CastMember>{ russCast, },
                },
            };
            foreach (var m in movies)
            {
                context.Movies.Add(m);
            }
            context.SaveChanges();
        }
    }
}
