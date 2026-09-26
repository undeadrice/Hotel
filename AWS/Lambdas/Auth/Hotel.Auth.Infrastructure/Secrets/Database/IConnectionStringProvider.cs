namespace Hotel.Auth.Infrastructure.Secrets.Database;

public interface IConnectionStringProvider
{
    Task<string> GetConnectionStringAsync(CancellationToken token = default);
}