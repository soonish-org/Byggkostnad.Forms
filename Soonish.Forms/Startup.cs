using Soonish.Forms.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MySQL.Data.EntityFrameworkCore.Extensions;
using GlobalPhone;
using Swashbuckle.AspNetCore.Swagger;

namespace Soonish.Forms
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<FormsDbContext>(options =>
                options.UseMySQL(Configuration.GetConnectionString("DefaultConnection")));
            services.AddOptions();
            services.Configure<AzureTableOptions>(Configuration.GetSection("AzureTable"));
            // Add framework services.
            services.AddMvc();
            services.AddSingleton(ioc => new Context()
            {
                DbText = PhoneNumbers.Data.Get(),
                DefaultTerritoryName = "se"
            });
            services.AddSwaggerGen(options => { });
            services.ConfigureSwaggerGen(options =>
            {
                options.SwaggerDoc("forms", new Info
                {
                    Version = "v1",
                    Title = "soonish",
                    Description = "",
                    TermsOfService = "None",
                    Contact = new Contact
                    {
                        Name = "Soonish",
                        Email = "developers@soonish.cloud",
                        Url = "http://soonish.cloud"
                    }
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            app.UseSwagger(options =>
            {
                options.RouteTemplate = "swagger/{documentName}/swagger.json";
            });

            // Enable middleware to serve swagger-ui assets (HTML, JS, CSS etc.)
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/forms/swagger.json", "Soonish Forms");
            });
            app.UseMvcWithDefaultRoute();
        }
    }
}
