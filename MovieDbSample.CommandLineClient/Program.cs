using GraphQL.Client;
using GraphQL.Common.Request;
using MovieDbSample.DataModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieDbSample.CommandLineClient
{
    class Program
    {
        private static readonly string ServerUrl = "https://localhost:44312/graphql";

        static async Task Main()
        {
            using var graphQLClient = new GraphQLClient(ServerUrl);

            // Basic GraphQL query

            var allMoviesResponse = await graphQLClient.PostQueryAsync(@"{ movies { title } }"); // TODO: Add 'year'
            Console.WriteLine($"Title of first movie: {allMoviesResponse.Data.movies[0].title}");

            var allMovies = allMoviesResponse.GetDataFieldAs<IEnumerable<Movie>>("movies");
            Console.WriteLine("All movies:");
            foreach (var movie in allMovies)
            {
                Console.WriteLine($"\t{movie.Title} ({movie.Year})");
            }


            // Parameterized GraphQL query

            var firstActorsInMoviesResponse = await graphQLClient.PostAsync(new GraphQLRequest
            {
                Query = @"query ($firstMovies: Int, $firstCast: Int, $genre: Genre!) {
                    movies(first: $firstMovies, genre: $genre) {
                        title
                        year
                        cast(first: $firstCast) {
                            character
                            actor {
                                name
                            }
                        }
                    }
                }",
                Variables = new
                {
                    firstMovies = 3, // optional
                    firstCast = 1, // optional
                    genre = "ACTION", // required
                },
            });

            var firstActorsInMovies = firstActorsInMoviesResponse.GetDataFieldAs<IEnumerable<Movie>>("movies");
            Console.WriteLine("First actors in movies:");
            foreach (var movie in firstActorsInMovies)
            {
                Console.WriteLine($"\t{movie.Title} ({movie.Year})");
                foreach (var castMember in movie.Cast)
                {
                    Console.WriteLine($"\t\t{castMember.Character}, played by {castMember.Actor.Name}");
                }
            }
        }
    }
}
