using Akka.Actor;
using Cik.Magazine.Core.Services;
using Cik.Magazine.Core.Storage.Query;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Swashbuckle.Swagger.Model;

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

            // Add swagger
            services.AddSwaggerGen(
                c =>
                {
                    c.SingleApiVersion(new Info {Version = "v1", Title = "Magazine Website API"});
                });

            services.AddScoped<IActorRefFactory>(serviceProvider => ActorSystem.Create("sys"));
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ICategoryQuery, CategoryQuery>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseMvc();

            app.UseSwagger();
            app.UseSwaggerUi();
        }
    }
}