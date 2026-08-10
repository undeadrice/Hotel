using Hotel.Application.Roles.Dtos;
using MediatR;

namespace Hotel.Application.Roles.Queries;

internal class GetPermissionsQueryHandler
    : IRequestHandler<GetPermissionsQuery, IReadOnlyCollection<PermissionGroupDto>>
{
    private static readonly IReadOnlyCollection<PermissionGroupDto> _groups =
    [
        new PermissionGroupDto("Role", ["RoleCreate", "RoleEdit", "RoleDelete", "RoleView"]),
        new PermissionGroupDto("User", ["UserCreate", "UserEdit", "UserDelete", "UserView"]),
        new PermissionGroupDto("Permissions", ["PermissionView"]),
        new PermissionGroupDto("Reservation", ["ReservationCreate", "ReservationView"]),
        new PermissionGroupDto("Room", ["RoomCreate", "RoomEdit", "RoomDelete", "RoomView"]),
        new PermissionGroupDto("RoomType", ["RoomTypeCreate", "RoomTypeEdit", "RoomTypeDelete", "RoomTypeView"]),
        new PermissionGroupDto("RatePlan", ["RatePlanCreate", "RatePlanEdit", "RatePlanDelete", "RatePlanView"]),
        new PermissionGroupDto("Guest", ["GuestCreate", "GuestEdit", "GuestDelete", "GuestView"]),
        new PermissionGroupDto("FiscalAccount", ["FiscalAccountEdit", "FiscalAccountView"]),
        new PermissionGroupDto("TransactionCode", ["TransactionCodeCreate", "TransactionCodeEdit", "TransactionCodeView"]),
        new PermissionGroupDto("TransactionGroup", ["TransactionGroupCreate", "TransactionGroupEdit", "TransactionGroupView"]),
        new PermissionGroupDto("Dashboard", ["DashboardView"]),
    ];

    public Task<IReadOnlyCollection<PermissionGroupDto>> Handle(GetPermissionsQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(_groups);
    }
}