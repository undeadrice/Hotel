using Hotel.Application.Seeding;
using Hotel.Infrastructure;
using Hotel.Persistence;
using MediatR;
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
    private const string TestTimeZoneId = "Greenwich Standard Time";

    private readonly string _dbName = "HotelTestDb_" + Guid.NewGuid().ToString("N");
    private readonly string _identityDbName = "HotelAuthTestDb_" + Guid.NewGuid().ToString("N");

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            var toRemove = services
                .Where(d =>
                    d.ServiceType == typeof(ISeedingService) ||
                    d.ServiceType == typeof(PersistenceDbContext) ||
                    d.ServiceType == typeof(DbContextOptions<PersistenceDbContext>) ||
                    d.ServiceType == typeof(InfraIdentityDbContext) ||
                    d.ServiceType == typeof(DbContextOptions<InfraIdentityDbContext>) ||
                    d.ServiceType == typeof(DbContextOptions) ||
                    (d.ServiceType.FullName?.StartsWith("Microsoft.EntityFrameworkCore") == true &&
                     (d.ServiceType.FullName.Contains("PersistenceDbContext") ||
                      d.ServiceType.FullName.Contains("InfraIdentityDbContext"))))
                .ToList();

            foreach (var d in toRemove)
            {
                services.Remove(d);
            }

            RemoveOptionsConfigurations<PersistenceDbContext>(services);
            RemoveOptionsConfigurations<InfraIdentityDbContext>(services);

            var connectionString = $"Server=(localdb)\\mssqllocaldb;Database={_dbName};Trusted_Connection=True;MultipleActiveResultSets=true";
            var identityConnectionString = $"Server=(localdb)\\mssqllocaldb;Database={_identityDbName};Trusted_Connection=True;MultipleActiveResultSets=true";

            services.AddDbContext<PersistenceDbContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddDbContext<InfraIdentityDbContext>(options =>
                options.UseSqlServer(identityConnectionString));

            services.AddScoped<ISeedingService, TestSeedingService>();
        });

        builder.UseEnvironment("Development");
    }

    private static void RemoveOptionsConfigurations<TContext>(IServiceCollection services)
        where TContext : DbContext
    {
        var optionsConfigType = typeof(IDbContextOptionsConfiguration<TContext>);
        var optionsConfigs = services
            .Where(d => d.ServiceType == optionsConfigType)
            .ToList();

        foreach (var d in optionsConfigs)
        {
            services.Remove(d);
        }
    }

    public async Task CreateDatabase()
    {
        using (var scope = Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<PersistenceDbContext>();
            await dbContext.Database.EnsureCreatedAsync();

            var identityContext = scope.ServiceProvider.GetRequiredService<InfraIdentityDbContext>();
            await identityContext.Database.EnsureCreatedAsync();
        }

        await InitializeApplicationAsync();
    }

    private async Task InitializeApplicationAsync()
    {
        using var scope = Services.CreateScope();
        var handler = scope.ServiceProvider.GetRequiredService<IRequestHandler<SeedDataCommand, Guid>>();

        var businessDate = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(1));

        var command = new SeedDataCommand(
            TestTimeZoneId,
            businessDate,
            SeedBusinessData: false);

        await handler.Handle(command, CancellationToken.None);
    }

    public async Task DeleteDatabase()
    {
        using var scope = Services.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<PersistenceDbContext>();
        await dbContext.Database.EnsureDeletedAsync();

        var identityContext = scope.ServiceProvider.GetRequiredService<InfraIdentityDbContext>();
        await identityContext.Database.EnsureDeletedAsync();
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