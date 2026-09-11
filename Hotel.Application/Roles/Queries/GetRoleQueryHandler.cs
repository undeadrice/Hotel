using Hotel.Application.Pipeline;
using Hotel.Application.Roles.Dtos;
using Hotel.Application.Roles.Services;
using Hotel.Application.Users.Enums;
using MediatR;



namespace Hotel.Application.Roles.Queries;



[CheckPermission(Permission.RoleView)]

public record GetRoleQuery(Guid Id) : IRequest<RoleDto>;



internal class GetRoleQueryHandler(IRoleService roleService)

    : IRequestHandler<GetRoleQuery, RoleDto>

{

    public async Task<RoleDto> Handle(GetRoleQuery request, CancellationToken cancellationToken)

    {

        return await roleService.Get(request.Id);

    }

}
