using Hotel.Application.Common;
using Hotel.Infrastructure.Common;
using Hotel.Shared.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Hotel.Infrastructure;

public static class DIRegistrations
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            var rsa = RSA.Create();
            rsa.ImportFromPem(configuration["jwt:PublicKey"]!);

            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration["jwt:Issuer"],
                ValidAudience = configuration["jwt:Audience"],
                IssuerSigningKey = new RsaSecurityKey(rsa),
                RoleClaimType = ClaimTypes.Role
            };
        });

        services.AddHttpContextAccessor();
        services.AddSharedInfrastructure();
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

        return services;
    }
}
