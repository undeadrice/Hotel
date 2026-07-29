using Hotel.Application.Pipeline;
using Hotel.Application.Users.Enums;

namespace Hotel.Application.Users.Commands;

[CheckPermission(Permission.UserEdit)]
public record UpdateUserCommand(
    Guid Id,
    string FirstName,
    string LastName,
    DateOnly DateOfBirth,
    string Email,
    IReadOnlyCollection<Guid> RoleIds) : ICommand;