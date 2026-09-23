using Hotel.SharedPipeline;
using Hotel.SharedPipeline.Attributes;
using Hotel.Application.Roles.Services;
using Hotel.Shared.Application.Users.Enums;
using MediatR;

namespace Hotel.Application.Roles.Commands;

[CheckPermission(Permission.RoleCreate)]
public record CreateRoleCommand(string Name, IReadOnlyCollection<string> Permissions) : ICommand<Guid>;

internal class CreateRoleCommandHandler(IRoleService roleService) : IRequestHandler<CreateRoleCommand, Guid>
{
    public async Task<Guid> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        var result = await roleService.Create(request.Name, request.Permissions);
        return result;
    }
}
