using Hotel.Application.Users.Enums;

namespace Hotel.Application.Auth.Services;

public interface ICurrentUserService
{
    Guid? CurrentUserId { get; }

    bool IsAuthenticated { get; }

    Task<bool> IsInRole(UserRole role);

    bool HasPermissions(params Permission[] permissions);

    Task<bool> IsSuperAdmin();
}