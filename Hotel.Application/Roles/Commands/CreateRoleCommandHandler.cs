using Hotel.Application.Pipeline;
using Hotel.Application.Roles.Services;
using Hotel.Application.Users.Enums;
using MediatR;

namespace Hotel.Application.Roles.Commands;

[CheckPermission(Permission.RoleCreate)]
public record CreateRoleCommand(string Name, IReadOnlyCollection<string> Permissions) : ICommand<Guid>;

public class CreateRoleCommandHandler(IRoleService roleService) : IRequestHandler<CreateRoleCommand, Guid>
{
    public async Task<Guid> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        var result = await roleService.Create(request.Name, request.Permissions);
        return result;
    }
}
