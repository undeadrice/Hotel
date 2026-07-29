using Hotel.Application.Roles.Dtos;
using Hotel.Application.Roles.Services;
using MediatR;

namespace Hotel.Application.Roles.Queries;

internal class GetRoleQueryHandler(IRoleService roleService)
    : IRequestHandler<GetRoleQuery, RoleDto>
{
    public async Task<RoleDto> Handle(GetRoleQuery request, CancellationToken cancellationToken)
    {
        return await roleService.Get(request.Id);
    }
}