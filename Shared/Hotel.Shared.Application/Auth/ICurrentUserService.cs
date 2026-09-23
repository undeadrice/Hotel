using Hotel.Shared.Application.Users.Enums;

namespace Hotel.Shared.Application.Auth;

public interface ICurrentUserService
{
    Guid? CurrentUserId { get; }

    bool IsAuthenticated { get; }

    Task<bool> IsInRole(UserRole role);

    bool HasPermissions(params Permission[] permissions);

    Task<bool> IsSuperAdmin();
}
