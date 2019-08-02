using AspNetCoreGraphQL.Server.DataAccess;
using AspNetCoreGraphQL.Server.DataModels;
using GraphQL;
using GraphQL.Types;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace AspNetCoreGraphQL.Server.GraphQLModels
{
    public class MoviesGraphQLSchema : Schema
    {
        public MoviesGraphQLSchema(IDependencyResolver resolver)
            : base(resolver)
        {
            Query = resolver.Resolve<MoviesDbQuery>();
            //Mutation = new MoviesDbMutation(...);
            //Subscription = new MoviesDbSubscriptions(...);
        }
    }

    public class MoviesDbQuery : ObjectGraphType
    {
        public MoviesDbQuery(MoviesDbContext moviesDbContext)
        {
            Name = "MoviesDb";
            Description = "A database of movies, actors, and characters.";

            Field<ListGraphType<MovieType>>(
                "movies",
                arguments: new QueryArguments(
                    new QueryArgument<GenreType>() { Name = "genre", Description = "Return only movies in this genre." },
                    new QueryArgument<IntGraphType>() { Name = "first", Description = "The number of items to include." }
                ),
                resolve: context =>
                {
                    var movies = moviesDbContext.Movies.AsEnumerable();
                    if (context.Arguments.ContainsKey("genre"))
                    {
                        var genre = context.GetArgument<Genre>("genre");
                        movies = movies.Where(m => m.Genre == genre);
                    }
                    if (context.Arguments.ContainsKey("first"))
                    {
                        movies = movies.Take(context.GetArgument<int>("first"));
                    }
                    return movies;
                },
                description: "All movies.");
            FieldAsync<MovieType>(
                "movie",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "id" }),
                resolve: async context =>
                {
                    var id = context.GetArgument<int>("id");
                    return await moviesDbContext.Movies.FindAsync(id);
                }
            );

            FieldAsync<CastMemberType>(
                "castmember",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "id" }),
                resolve: async context =>
                {
                    var id = context.GetArgument<int>("id");
                    return await moviesDbContext.CastMembers.FindAsync(id);
                }
            );

            FieldAsync<ActorType>(
                "actor",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "id" }),
                resolve: async context =>
                {
                    var id = context.GetArgument<int>("id");
                    return await moviesDbContext.Actors.FindAsync(id);
                }
            );
        }
    }

    public class MovieType : ObjectGraphType<Movie>
    {
        public MovieType(MoviesDbContext moviesDbContext)
        {
            Field(x => x.ID, type: typeof(IdGraphType)).Name("id");

            Field(x => x.Title, nullable: false).Description("Movie title.");
            Field(x => x.Year).Description("Movie release year.");
            Field(x => x.Language).Description("Movie language.");
            Field(x => x.Genre, type: typeof(GenreType)).Description("Movie genre.");

            Field<ListGraphType<CastMemberType>>(
                "cast",
                arguments: new QueryArguments(
                    new QueryArgument<IntGraphType>() { Name = "first", Description = "The number of items to include." }),
                description: "The cast of the movie",
                resolve: context =>
                {
                    var cast = moviesDbContext.Movies.Include(m => m.Cast).SingleOrDefault(m => m.ID == context.Source.ID).Cast.AsEnumerable();
                    if (context.Arguments.ContainsKey("first"))
                    {
                        cast = cast.Take(context.GetArgument<int>("first"));
                    }
                    return cast;
                });
        }
    }

    public class GenreType : EnumerationGraphType<Genre>
    {
    }

    public class CastMemberType : ObjectGraphType<CastMember>
    {
        public CastMemberType(MoviesDbContext moviesDbContext)
        {
            Field(x => x.ID, type: typeof(IdGraphType)).Name("id");

            Field(x => x.Character, nullable: false).Description("Character name.");
            Field(x => x.OrderOfAppearance).Description("Order of appearance in the movie.");

            Field(x => x.Actor, type: typeof(ActorType))
                .Resolve(context => moviesDbContext.CastMembers.Include(c => c.Actor).SingleOrDefault(c => c.ID == context.Source.ID).Actor)
                .Description("The actor playing the role.");
        }
    }

    public class ActorType : ObjectGraphType<Actor>
    {
        public ActorType()
        {
            Field(x => x.ID, type: typeof(IdGraphType)).Name("id");

            Field(x => x.Name, nullable: false).Description("Actor's name.");
            Field(x => x.DOB).Description("Actor's date of birth.");
        }
    }
}
