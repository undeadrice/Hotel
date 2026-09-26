namespace Hotel.Auth.Infrastructure.Secrets;

public interface IConnectionStringProvider
{
    Task<string> GetConnectionStringAsync(CancellationToken token = default);
}