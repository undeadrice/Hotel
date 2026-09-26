using Hotel.Auth.Infrastructure.Auth.Contracts;
using Hotel.Auth.Application.Auth.Services;
using Hotel.Auth.Application.Roles.Services;
using Hotel.Auth.Application.Users.Services;
using Hotel.Auth.Infrastructure.Auth.Entities;
using Hotel.Auth.Infrastructure.Auth.Services;
using Hotel.Shared.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Hotel.Auth.Application.Initialization;
using Hotel.Auth.Infrastructure.Initialization;
using Amazon.SecretsManager;
using Hotel.Auth.Infrastructure.Secrets.Database;
using Hotel.Auth.Infrastructure.Secrets.Jwt;

namespace Hotel.Auth.Infrastructure;

public static class DIRegistrations
{
    public static IServiceCollection AddAuthInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IAmazonSecretsManager>(new AmazonSecretsManagerClient());

        services.AddSingleton<IConnectionStringProvider>(sp =>
        {
            var client = sp.GetRequiredService<IAmazonSecretsManager>();
            var connectionSecretId = configuration["SecretsManager:ConnectionSecretId"]
                ?? throw new InvalidOperationException("SecretsManager:ConnectionSecretId is not configured.");
            var credentialsSecretId = configuration["SecretsManager:CredentialsSecretId"]
                ?? throw new InvalidOperationException("SecretsManager:CredentialsSecretId is not configured.");

            return new SecretsManagerConnectionStringProvider(client, connectionSecretId, credentialsSecretId);
        });

        services.AddSingleton<IJwtPrivateKeyProvider>(sp =>
        {
            var client = sp.GetRequiredService<IAmazonSecretsManager>();
            var jwtPrivateKeySecretId = configuration["SecretsManager:JwtPrivateKeySecretId"]
                ?? throw new InvalidOperationException("SecretsManager:JwtPrivateKeySecretId is not configured.");

            return new SecretsManagerJwtPrivateKeyProvider(client, jwtPrivateKeySecretId);
        });

        services.AddDbContext<InfraIdentityDbContext>((serviceProvider, options) =>
        {
            var secretProvider = serviceProvider.GetRequiredService<IConnectionStringProvider>();
            var connectionString = secretProvider.GetConnectionStringAsync().GetAwaiter().GetResult();

            options.UseSqlServer(connectionString);
        });

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

        services.AddIdentityCore<ApplicationUser>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequiredLength = 3;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireLowercase = false;
            options.Password.RequiredUniqueChars = 0;
            options.SignIn.RequireConfirmedEmail = false;
        })
            .AddRoles<ApplicationRole>()
            .AddEntityFrameworkStores<InfraIdentityDbContext>()
            .AddDefaultTokenProviders();

        services.AddHttpContextAccessor();

        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IAuthService, AuthService>();

        services.AddScoped<IInitializationService, InitializationService>();

        services.AddSharedInfrastructure();

        services.AddSingleton(sp =>
        {
            var privateKey = sp.GetRequiredService<IJwtPrivateKeyProvider>()
                .GetPrivateKeyAsync()
                .GetAwaiter()
                .GetResult();

            return new JwtSettings(
                privateKey,
                configuration["jwt:Issuer"]!,
                configuration["jwt:Audience"]!
            );
        });

        return services;
    }
}
