namespace Hotel.Auth.Application.Auth.Models;

public record JwtSettings(string Secret, string Issuer, string Audience);