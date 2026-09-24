using Hotel.SharedPipeline.Attributes;
using Hotel.Auth.Application.Roles.Dtos;
using Hotel.Auth.Application.Roles.Services;
using Hotel.Shared.Application.Users.Enums;
using MediatR;

namespace Hotel.Auth.Application.Roles.Queries;

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
