namespace Hotel.Auth.Infrastructure.Secrets;

public interface IJwtPrivateKeyProvider
{
    Task<string> GetPrivateKeyAsync(CancellationToken token = default);
}
