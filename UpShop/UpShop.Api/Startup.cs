using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Text;
using UpShop.Api.Auth;
using UpShop.Api.Utils;
using UpShop.DAL;
using UpShop.Dominio.Interfaces;

namespace UpShop.Api
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
            // Add DB service
            services.AddSingleton<IRepository>(Inject.GetRepositoryImplemantation(Configuration["ConnectionString"]));

            // Add Auth services.
            var key = Encoding.UTF8.GetBytes(Configuration["Auth:SecurityKey"]);
            var expiration = Int32.Parse(Configuration["Auth:Expiration"]);
            services.AddSingleton<JwtSettings>(new JwtSettings(new SymmetricSecurityKey(key), expiration));

            services.AddSingleton<AuthenticationHandler>();
            services.AddTransient<JwtSecurityTokenHandler>();
            services.AddTransient<JwtProvider>();
            services.AddTransient<Encryptor>();

            // Register the Swagger generator, defining one or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "UpShop API",
                    Description = "A simple api for admission exame.",
                    TermsOfService = "None",
                    Contact = new Contact { Name = "Filipe Santiago", Email = "porto.santiago@hotmail.com" },
                });

                // Set the Bearer token for Authorization in Swagger UI
                c.AddSecurityDefinition("Bearer", new ApiKeyScheme() { In = "header", Description = "Please insert JWT with Bearer into field", Name = "Authorization", Type = "apiKey" });

                // Set the comments path for the Swagger JSON and UI.
                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var xmlPath = Path.Combine(basePath, "UpShop.Api.xml");
                c.IncludeXmlComments(xmlPath);
            });

            // Add framework services.
            // Configure JSON to read/write ObjectID types.
            services.AddMvc().AddJsonOptions(opt => { opt.SerializerSettings.Converters.Add(new ObjectIdConverter()); });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, JwtSettings jwtSettings, AuthenticationHandler handler)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            var validationParameters = new TokenValidationParameters
            {
                IssuerSigningKey = jwtSettings.SecurityKey,
                ValidAudience = jwtSettings.Audience,
                ValidIssuer = jwtSettings.Issuer
            };

            var options = new JwtBearerOptions
            {
                Events = handler,
                TokenValidationParameters = validationParameters
            };

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "UpShop API V1");
            });

            app.UseJwtBearerAuthentication(options);
            app.UseMvc();
        }
    }
}
