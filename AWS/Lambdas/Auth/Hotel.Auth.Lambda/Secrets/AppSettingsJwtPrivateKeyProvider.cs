using Hotel.Auth.Infrastructure.Secrets;

namespace Hotel.Auth.Lambda.Secrets;

internal sealed class AppSettingsJwtPrivateKeyProvider : IJwtPrivateKeyProvider
{
    private readonly IConfiguration _configuration;

    public AppSettingsJwtPrivateKeyProvider(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public Task<string> GetPrivateKeyAsync(CancellationToken token = default)
    {
        var privateKey = _configuration["jwt:PrivateKey"]
            ?? throw new InvalidOperationException("jwt:PrivateKey is not configured.");

        return Task.FromResult(privateKey);
    }
}
