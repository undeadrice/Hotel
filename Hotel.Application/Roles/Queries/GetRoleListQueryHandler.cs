using Hotel.Application.Roles.Dtos;
using Hotel.Application.Roles.Services;
using MediatR;

namespace Hotel.Application.Roles.Queries;

internal class GetRoleListQueryHandler(IRoleService roleService)
    : IRequestHandler<GetRoleListQuery, IReadOnlyCollection<RoleSimpleDto>>
{
    public async Task<IReadOnlyCollection<RoleSimpleDto>> Handle(GetRoleListQuery request, CancellationToken cancellationToken)
    {
        return await roleService.GetAll();
    }
}