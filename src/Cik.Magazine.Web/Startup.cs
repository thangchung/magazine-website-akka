using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using Akka.Actor;
using Akka.Routing;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;

namespace Cik.Magazine.Web
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services
                .AddMvc()
                .AddJsonOptions(
                    opts =>
                    {
                        opts.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                        opts.SerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
                        opts.SerializerSettings.Formatting = Formatting.Indented;
                        opts.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    });

            // services.AddAuthorization();

            // Add swagger
            services.AddSwaggerGen(
                c =>
                {
                    c.SwaggerDoc("v1", new Info
                    {
                        Version = "v1",
                        Title = "Magazine Website API"
                    });
                    /* c.AddSecurityDefinition("oauth2", new OAuth2Scheme
                    {
                        Type = "oauth2",
                        Flow = "implicit",
                        TokenUrl = "http://localhost:9999/connect/token",
                        AuthorizationUrl = "http://localhost:9999/connect/authorize",
                        Scopes = new Dictionary<string, string>{ { "magazine-api", "Magazine API Resource" } }
                    });
                    c.OperationFilter<SecurityRequirementsOperationFilter>(); */
                    c.DescribeAllEnumsAsStrings();

                    // Set the comments path for the swagger json and ui.
                    var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                    var xmlPath = Path.Combine(basePath, "MagazineApi.xml");
                    c.IncludeXmlComments(xmlPath);
                });

            // build up the actors
            var systemActor = ActorSystem.Create("magazine-system");
            systemActor.ActorOf(Props.Empty.WithRouter(FromConfig.Instance), "category-query-group");
            systemActor.ActorOf(Props.Empty.WithRouter(FromConfig.Instance), "category-commander-group");

            services.AddSingleton<IActorRefFactory>(serviceProvider => systemActor);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            /* JwtSecurityTokenHandler.DefaultInboundClaimFilter.Clear();
            app.UseIdentityServerAuthentication(new IdentityServerAuthenticationOptions 
            {
                Authority = "http://localhost:9999/",
                RequireHttpsMetadata = false // don't use this for production env
            }); */  

            app.UseMvc();

            app.UseSwagger();
            app.UseSwaggerUi(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Magazine Website API V1");
                // c.ConfigureOAuth2("swagger", "secret".Sha256(), "swagger", "swagger");
            });
        }
    }
}