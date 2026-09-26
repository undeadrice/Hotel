using Hotel.Auth.Application;
using Hotel.Auth.Application.Initialization;
using Hotel.Auth.Application.Seeding;
using Hotel.Auth.Infrastructure;
using Hotel.Auth.Infrastructure.Secrets.Database;
using Hotel.Auth.Infrastructure.Secrets.Jwt;
using Hotel.Auth.Lambda.Secrets.Database;
using Hotel.Auth.Lambda.Secrets.Jwt;
using Hotel.Shared.API.Middleware;

namespace Hotel.Auth.Lambda
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddAuthApplication();
            services.AddAuthInfrastructure(Configuration);

#if DEBUG
            services.AddSingleton<IConnectionStringProvider, AppSettingsConnectionStringProvider>();
            services.AddSingleton<IJwtPrivateKeyProvider, AppSettingsJwtPrivateKeyProvider>();
#endif

            services.AddExceptionHandler<DomainExceptionHandler>();
            services.AddProblemDetails();

            services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend", policy =>
                {
                    policy.WithOrigins("http://localhost:4200")
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseExceptionHandler();

            using (var scope = app.ApplicationServices.CreateScope())
            {
                var initializationService = scope.ServiceProvider.GetRequiredService<IInitializationService>();
                initializationService.Initialize();

                var seedingService = scope.ServiceProvider.GetRequiredService<ISeedingService>();
                seedingService.SeedAsync().GetAwaiter().GetResult();
            }

            if (!env.IsDevelopment())
            {
                app.UseHttpsRedirection();
            }

            app.UseRouting();
            app.UseCors("AllowFrontend");
            app.UseAuthentication();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Welcome to running ASP.NET Core on AWS Lambda");
                });
            });
        }
    }
}