using Hotel.Auth.Infrastructure.Secrets;

namespace Hotel.Auth.Lambda.Secrets;

internal sealed class AppSettingsDbConnectionSecretProvider : IDbConnectionSecretProvider
{
    private readonly IConfiguration _configuration;

    public AppSettingsDbConnectionSecretProvider(IConfiguration configuration)
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
