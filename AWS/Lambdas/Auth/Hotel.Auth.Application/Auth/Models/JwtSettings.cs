namespace Hotel.Auth.Application.Auth.Models;

public record JwtSettings(string PrivateKey, string Issuer, string Audience);