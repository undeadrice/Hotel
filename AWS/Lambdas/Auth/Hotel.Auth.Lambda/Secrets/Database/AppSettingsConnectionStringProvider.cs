using Hotel.Auth.Infrastructure.Secrets.Database;

namespace Hotel.Auth.Lambda.Secrets.Database;

internal sealed class AppSettingsConnectionStringProvider : IConnectionStringProvider
{
    private readonly IConfiguration _configuration;

    public AppSettingsConnectionStringProvider(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public Task<string> GetConnectionStringAsync(CancellationToken token = default)
    {
        var connectionString = _configuration.GetConnectionString("IdentityConnection")
            ?? throw new InvalidOperationException("ConnectionStrings:IdentityConnection is not configured.");

        return Task.FromResult(connectionString);
    }
}
