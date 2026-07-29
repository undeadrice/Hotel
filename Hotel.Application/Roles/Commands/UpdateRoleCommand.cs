using Hotel.Application.Pipeline;
using Hotel.Application.Users.Enums;

namespace Hotel.Application.Roles.Commands;

[CheckPermission(Permission.RoleEdit)]
public record UpdateRoleCommand(Guid Id, string Name, IReadOnlyCollection<string> Permissions) : ICommand;