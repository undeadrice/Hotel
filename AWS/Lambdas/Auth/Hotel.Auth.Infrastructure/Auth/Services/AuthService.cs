using Hotel.Auth.Application.Auth.Models;
using Hotel.Auth.Application.Auth.Services;
using Hotel.Auth.Infrastructure.Auth.Entities;
using Hotel.Shared.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Hotel.Auth.Infrastructure.Auth.Services;

internal class AuthService(
    UserManager<ApplicationUser> userManager,
    InfraIdentityDbContext dbContext,
    JwtSettings jwtSettings) : IAuthService
{
    public async Task<string> Login(string email, string password)
    {
        var user = await userManager.FindByEmailAsync(email);

        if (user == null || !await userManager.CheckPasswordAsync(user, password))
        {
            throw new UnauthorizedException("Invalid credentials");
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email!),
            new Claim(ClaimTypes.GivenName, user.FirstName),
            new Claim(ClaimTypes.Surname, user.LastName)
        };

        var roles = await userManager.GetRolesAsync(user);

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var roleIds = await dbContext.Roles
            .Where(r => roles.Contains(r.Name!))
            .Select(r => r.Id)
            .ToListAsync();

        var permissions = await dbContext.RoleClaims
            .Where(rc => roleIds.Contains(rc.RoleId) && rc.ClaimType == "permission" && rc.ClaimValue != null)
            .Select(rc => rc.ClaimValue!)
            .Distinct()
            .ToListAsync();

        foreach (var permission in permissions)
        {
            claims.Add(new Claim("permission", permission));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: jwtSettings.Issuer,
            audience: jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddDays(20),
            signingCredentials: creds
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
        return tokenString;
    }
}
