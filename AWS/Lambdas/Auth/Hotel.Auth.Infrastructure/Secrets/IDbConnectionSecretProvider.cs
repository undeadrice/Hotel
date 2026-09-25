namespace Hotel.Auth.Infrastructure.Secrets;

internal interface IDbConnectionSecretProvider
{
    Task<DbConfig> GetDbConfigAsync(CancellationToken token = default);
}