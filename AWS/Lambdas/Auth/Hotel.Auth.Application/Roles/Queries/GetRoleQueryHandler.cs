using Hotel.SharedPipeline.Attributes;
using Hotel.Auth.Application.Roles.Dtos;
using Hotel.Auth.Application.Roles.Services;
using Hotel.Shared.Application.Users.Enums;
using MediatR;

namespace Hotel.Auth.Application.Roles.Queries;

[CheckPermission(Permission.RoleView)]
public record GetRoleQuery(Guid Id) : IRequest<RoleDto>;

internal class GetRoleQueryHandler(IRoleService roleService) : IRequestHandler<GetRoleQuery, RoleDto>
{
    public async Task<RoleDto> Handle(GetRoleQuery request, CancellationToken cancellationToken)
    {
        return await roleService.Get(request.Id);
    }
}