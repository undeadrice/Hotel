using Hotel.Application.Pipeline;
using Hotel.Application.Users.Enums;

namespace Hotel.Application.Roles.Commands;

[CheckPermission(Permission.RoleCreate)]
public record CreateRoleCommand(string Name, IReadOnlyCollection<string> Permissions) : ICommand<Guid>;