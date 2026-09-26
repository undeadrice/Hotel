namespace Hotel.Auth.Infrastructure.Secrets.Jwt;

public interface IJwtPrivateKeyProvider
{
    Task<string> GetPrivateKeyAsync(CancellationToken token = default);
}
