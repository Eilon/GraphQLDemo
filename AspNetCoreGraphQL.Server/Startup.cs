using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreGraphQL.Server.DataAccess;
using AspNetCoreGraphQL.Server.GraphQLModels;
using GraphQL;
using GraphQL.Server;
using GraphQL.Server.Ui.GraphiQL;
using GraphQL.Server.Ui.Playground;
using GraphQL.Server.Ui.Voyager;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AspNetCoreGraphQL.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            HostingEnvironment = hostingEnvironment;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment HostingEnvironment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<MoviesDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("MoviesDb"));
            });
            
            services.AddRazorPages();

            // Temporary workaround. See https://github.com/graphql-dotnet/graphql-dotnet/issues/1116
            services.Configure<IISServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });


            // GraphQL
            services.AddScoped<IDependencyResolver>(s => new FuncDependencyResolver(s.GetRequiredService));
            services.AddScoped<MoviesGraphQLSchema>();

            services.AddGraphQL(options =>
                {
                    options.EnableMetrics = true;
                    options.ExposeExceptions = HostingEnvironment.IsDevelopment();
                })
                .AddGraphTypes(ServiceLifetime.Scoped)
                .AddRelayGraphTypes();



        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            // use HTTP middleware at path /graphql
            app.UseGraphQL<MoviesGraphQLSchema>("/graphql");

            // use graphiQL middleware at default url /graphiql
            app.UseGraphiQLServer(new GraphiQLOptions());

            // use graphql-playground middleware at default url /ui/playground
            app.UseGraphQLPlayground(new GraphQLPlaygroundOptions());

            // use voyager middleware at default url /ui/voyager
            app.UseGraphQLVoyager(new GraphQLVoyagerOptions());


            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
