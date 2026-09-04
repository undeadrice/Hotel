using Hotel.Application.Pipeline;
using Hotel.Application.Roles.Services;
using Hotel.Application.Users.Enums;
using MediatR;

namespace Hotel.Application.Roles.Commands;


[CheckPermission(Permission.RoleEdit)]
public record UpdateRoleCommand(Guid Id, string Name, IReadOnlyCollection<string> Permissions) : ICommand;

public class UpdateRoleCommandHandler(IRoleService roleService) : IRequestHandler<UpdateRoleCommand>
{
    public async Task Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
    {
        await roleService.Update(request.Id, request.Name, request.Permissions);
    }
}