using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using System.Text.Json;

namespace Hotel.Auth.Infrastructure.Secrets;

internal class SecretsManagerConnectionProvider : IDbConnectionSecretProvider
{
    private readonly IAmazonSecretsManager _client;
    private readonly string _connectionSecretId;
    private readonly string _credentialsSecretId;
     
    private DbConfig? cached;

    public SecretsManagerConnectionProvider(
        IAmazonSecretsManager client,
        string connectionSecretId,
        string credentialsSecretId)
    {
        _client = client;
        _connectionSecretId = connectionSecretId;
        _credentialsSecretId = credentialsSecretId;
    }

    public async Task<DbConfig> GetDbConfigAsync(CancellationToken ct = default)
    {
        if (cached != null)
        {
            return cached;
        }

        var connectionTask = _client.GetSecretValueAsync(new GetSecretValueRequest { SecretId = _connectionSecretId }, ct);
        var credentialsTask = _client.GetSecretValueAsync(new GetSecretValueRequest { SecretId = _credentialsSecretId }, ct);

        await Task.WhenAll(connectionTask, credentialsTask);

        var jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        var connection = JsonSerializer.Deserialize<ConnectionInfoSecret>(connectionTask.Result.SecretString, jsonOptions)!;
        var credentials = JsonSerializer.Deserialize<CredentialsSecret>(credentialsTask.Result.SecretString, jsonOptions)!;

        cached = new DbConfig(
            connection.Host,
            connection.Port,
            connection.Dbname,
            credentials.Username,
            credentials.Password
        );

        return cached;
    }

    private record ConnectionInfoSecret(string Host, int Port, string Dbname);
    private record CredentialsSecret(string Username, string Password);
}