namespace Hotel.Infrastructure.Auth;

public record JwtSettings(string PrivateKey, string Issuer, string Audience);
