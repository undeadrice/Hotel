using Hotel.Application.Seeding;
using Hotel.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace Hotel.IntegrationTests.Infrastructure;

public class HotelWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly string _dbName = "HotelTestDb_" + Guid.NewGuid().ToString("N");

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            var toRemove = services
                .Where(d =>
                    d.ServiceType == typeof(ISeedingService) ||
                    d.ServiceType == typeof(PersistenceDbContext) ||
                    d.ServiceType == typeof(DbContextOptions<PersistenceDbContext>) ||
                    d.ServiceType == typeof(DbContextOptions) ||
                    d.ServiceType.FullName?.StartsWith("Microsoft.EntityFrameworkCore") == true &&
                    d.ServiceType.FullName.Contains("PersistenceDbContext"))
                .ToList();

            foreach (var d in toRemove)
            {
                services.Remove(d);
            }

            var optionsConfigType = typeof(IDbContextOptionsConfiguration<PersistenceDbContext>);
            var optionsConfigs = services
                .Where(d => d.ServiceType == optionsConfigType)
                .ToList();

            foreach (var d in optionsConfigs)
            {
                services.Remove(d);
            }

            var connectionString = $"Server=(localdb)\\mssqllocaldb;Database={_dbName};Trusted_Connection=True;MultipleActiveResultSets=true";

            services.AddDbContext<PersistenceDbContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddScoped<ISeedingService, TestSeedingService>();
        });

        builder.UseEnvironment("Development");
    }

    public async Task CreateDatabase()
    {
        using var scope = Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<PersistenceDbContext>();
        await dbContext.Database.EnsureCreatedAsync();
    }

    public async Task DeleteDatabase()
    {
        using var scope = Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<PersistenceDbContext>();
        await dbContext.Database.EnsureDeletedAsync();
    }

    public async Task<HttpClient> CreateAuthenticatedClientAsync(
        string email = "sa@sa.pl",
        string password = "Admin123!")
    {
        var client = CreateClient();

        var loginResponse = await client.PostAsJsonAsync(
            "/api/auth/login",
            new { email, password });

        loginResponse.EnsureSuccessStatusCode();

        using var json = JsonDocument.Parse(await loginResponse.Content.ReadAsStringAsync());
        var token = json.RootElement.GetProperty("token").GetString();

        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        return client;
    }
}
