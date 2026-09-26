using Hotel.Auth.Application;
using Hotel.Auth.Application.Initialization;
using Hotel.Auth.Application.Seeding;
using Hotel.Auth.Infrastructure;
using Hotel.Auth.Infrastructure.Secrets.Database;
using Hotel.Auth.Infrastructure.Secrets.Jwt;
using Hotel.Auth.Lambda.Secrets;
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
            services.AddSingleton<IConnectionStringProvider, AppSettingsDbConnectionSecretProvider>();
            services.AddSingleton<IJwtPrivateKeyProvider, AppSettingsJwtPrivateKeyProvider>();
#endif

            services.AddExceptionHandler<DomainExceptionHandler>();
            services.AddProblemDetails();
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

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
           
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