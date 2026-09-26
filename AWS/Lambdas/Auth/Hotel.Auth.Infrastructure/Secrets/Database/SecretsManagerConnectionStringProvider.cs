using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using System.Text.Json;
using Microsoft.Data.SqlClient;

namespace Hotel.Auth.Infrastructure.Secrets.Database;

internal class SecretsManagerConnectionStringProvider : IConnectionStringProvider
{
    private readonly IAmazonSecretsManager _client;
    private readonly string _connectionSecretId;
    private readonly string _credentialsSecretId;
     
    private string? _cachedConnectionString;

    public SecretsManagerConnectionStringProvider(
        IAmazonSecretsManager client,
        string connectionSecretId,
        string credentialsSecretId)
    {
        _client = client;
        _connectionSecretId = connectionSecretId;
        _credentialsSecretId = credentialsSecretId;
    }

    public async Task<string> GetConnectionStringAsync(CancellationToken ct = default)
    {
        if (_cachedConnectionString != null)
        {
            return _cachedConnectionString;
        }

        var connectionTask = _client.GetSecretValueAsync(new GetSecretValueRequest { SecretId = _connectionSecretId }, ct);
        var credentialsTask = _client.GetSecretValueAsync(new GetSecretValueRequest { SecretId = _credentialsSecretId }, ct);

        await Task.WhenAll(connectionTask, credentialsTask);

        var jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        var connection = JsonSerializer.Deserialize<ConnectionInfoSecret>(connectionTask.Result.SecretString, jsonOptions)!;
        var credentials = JsonSerializer.Deserialize<CredentialsSecret>(credentialsTask.Result.SecretString, jsonOptions)!;

        var connectionString = new SqlConnectionStringBuilder
        {
            DataSource = $"{connection.Host},{connection.Port}",
            InitialCatalog = connection.Dbname,
            UserID = credentials.Username,
            Password = credentials.Password,
            MultipleActiveResultSets = true,
            TrustServerCertificate = true
        }.ConnectionString;

        _cachedConnectionString = connectionString;

        return connectionString;
    }

    private record ConnectionInfoSecret(string Host, int Port, string Dbname);
    private record CredentialsSecret(string Username, string Password);
}