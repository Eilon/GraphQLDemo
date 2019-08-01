using AspNetCoreGraphQL.Server.DataAccess;
using AspNetCoreGraphQL.Server.DataModels;
using GraphQL;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

            Field<ListGraphType<MovieType>>("movies", resolve: context => moviesDbContext.Movies, description: "All movies.");

            FieldAsync<MovieType>(
                "movie",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "id" }),
                resolve: async context =>
                {
                    var id = context.GetArgument<int>("id");
                    return await moviesDbContext.Movies.FindAsync(id);
                }
            );

        }
    }

    public class MovieType : ObjectGraphType<Movie>
    {
        public MovieType()
        {
            Field(x => x.ID, type: typeof(IdGraphType)).Name("id");

            Field(x => x.Title).Description("Movie title.");
            Field(x => x.Year).Description("Movie release year.");
            Field(x => x.Language).Description("Movie language.");

            //Field(x => x.Genre, type: typeof(GenreType)).Description("Movie genre.");

        }
    }
}
