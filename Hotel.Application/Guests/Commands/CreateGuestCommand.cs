using Hotel.Application.Pipeline;
using Hotel.Application.Users.Enums;

namespace Hotel.Application.Guests.Commands;

[CheckRole(UserRole.Admin)]
public record CreateGuestCommand(
    string FirstName,
    string LastName,
    string Phone,
    string Email,
    string DocumentNumber) : ICommand<Guid>;