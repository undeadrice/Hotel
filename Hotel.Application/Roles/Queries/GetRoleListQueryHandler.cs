using Hotel.Application.Pipeline;
using Hotel.Application.Roles.Dtos;
using Hotel.Application.Roles.Services;
using Hotel.Application.Users.Enums;
using MediatR;

namespace Hotel.Application.Roles.Queries;

[CheckPermission(Permission.RoleView)]
public record GetRoleListQuery() : IRequest<IReadOnlyCollection<RoleSimpleDto>>;
internal class GetRoleListQueryHandler(IRoleService roleService)
    : IRequestHandler<GetRoleListQuery, IReadOnlyCollection<RoleSimpleDto>>
{
    public async Task<IReadOnlyCollection<RoleSimpleDto>> Handle(GetRoleListQuery request, CancellationToken cancellationToken)
    {
        return await roleService.GetAll();
    }
}
