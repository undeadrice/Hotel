namespace Hotel.Auth.Infrastructure.Secrets;

public interface IDbConnectionSecretProvider
{
    Task<string> GetConnectionStringAsync(CancellationToken token = default);
}