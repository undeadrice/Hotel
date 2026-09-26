namespace Hotel.Auth.Infrastructure.Auth.Contracts;

public record JwtSettings(string PrivateKey, string Issuer, string Audience);