using Hotel.SharedPipeline;
using Hotel.SharedPipeline.Attributes;
using Hotel.Application.Roles.Services;
using Hotel.Shared.Application.Users.Enums;
using MediatR;

namespace Hotel.Application.Roles.Commands;


[CheckPermission(Permission.RoleEdit)]
public record UpdateRoleCommand(Guid Id, string Name, IReadOnlyCollection<string> Permissions) : ICommand;

internal class UpdateRoleCommandHandler(IRoleService roleService) : IRequestHandler<UpdateRoleCommand>
{
    public async Task Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
    {
        await roleService.Update(request.Id, request.Name, request.Permissions);
    }
}