using Hotel.Auth.Application.Auth.Models;
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
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Hotel.Auth.Application.Initialization;
using Hotel.Auth.Infrastructure.Initialization;
using Hotel.Auth.Infrastructure.Secrets;

namespace Hotel.Auth.Infrastructure;

public static class DIRegistrations
{
    public static IServiceCollection AddAuthInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<InfraIdentityDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("IdentityConnection")));

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration["jwt:Issuer"],
                ValidAudience = configuration["jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["jwt:Secret"]!)),
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

        services.AddSingleton<IDbConnectionSecretProvider, SecretsManagerConnectionProvider>();

        services.AddSharedInfrastructure();

        var jwtSettings = new JwtSettings(
            configuration["jwt:Secret"]!,
            configuration["jwt:Issuer"]!,
            configuration["jwt:Audience"]!
        );
        services.AddSingleton(jwtSettings);

        return services;
    }
}
