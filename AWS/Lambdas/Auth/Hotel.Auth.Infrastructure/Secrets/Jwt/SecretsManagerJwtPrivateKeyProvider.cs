using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using System.Text.Json;

namespace Hotel.Auth.Infrastructure.Secrets.Jwt;

internal class SecretsManagerJwtPrivateKeyProvider : IJwtPrivateKeyProvider
{
    private readonly IAmazonSecretsManager _client;
    private readonly string _secretId;

    private string? _cachedPrivateKey;

    public SecretsManagerJwtPrivateKeyProvider(IAmazonSecretsManager client, string secretId)
    {
        _client = client;
        _secretId = secretId;
    }

    public async Task<string> GetPrivateKeyAsync(CancellationToken token = default)
    {
        if (_cachedPrivateKey != null)
        {
            return _cachedPrivateKey;
        }

        var response = await _client.GetSecretValueAsync(new GetSecretValueRequest { SecretId = _secretId }, token);

        var secretString = response.SecretString
            ?? throw new InvalidOperationException($"JWT private key secret '{_secretId}' is empty or missing.");

        _cachedPrivateKey = ExtractPrivateKey(secretString);

        return _cachedPrivateKey;
    }

    private static string ExtractPrivateKey(string secretString)
    {
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var secret = JsonSerializer.Deserialize<JwtPrivateKeySecret>(secretString, options);

        return secret?.PrivateKey ?? throw new InvalidOperationException("JWT private key secret JSON is missing the 'PrivateKey' property.");
    }

    private record JwtPrivateKeySecret(string PrivateKey);
}
