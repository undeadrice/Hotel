using Hotel.Application.Auth.Services;
using Hotel.Application.Users.Enums;
using Microsoft.AspNetCore.Http;

namespace Hotel.Infrastructure.Auth.Services;

internal class CurrentUserService(
    IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    public Guid? CurrentUserId
    {
        get
        {
            var userIdClaim = httpContextAccessor.HttpContext?.User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            return userIdClaim is not null ? Guid.Parse(userIdClaim) : null;
        }
    }

    public bool IsAuthenticated => httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;

    public async Task<bool> IsInRole(UserRole role)
    {
        var user = httpContextAccessor.HttpContext?.User;
        if (user is null) return false;

        return await Task.FromResult(user.IsInRole(role.ToString()));
    }

    public async Task<bool> IsSuperAdmin()
    {
        var user = httpContextAccessor.HttpContext?.User;
        if (user is null) return false;

        return await Task.FromResult(user.IsInRole(UserRole.SuperAdmin.ToString()));
    }

    public bool HasPermissions(params Permission[] permissions)
    {
        var user = httpContextAccessor.HttpContext?.User;
        if (user is null)
        {
            return false;
        }

        var permissionClaims = user.FindAll("permission")
            .Select(c => c.Value)
            .ToHashSet();

        foreach (var permission in permissions)
        {
            if (!permissionClaims.Contains(permission.ToString()))
            {
                return false;
            }
        }

        return true;
    }
}