using Hotel.Application.Pipeline;
using Hotel.Application.Users.Enums;

namespace Hotel.Application.Guests.Commands;

[CheckPermission(Permission.GuestCreate)]
public record CreateGuestCommand(
    string FirstName,
    string LastName,
    string Phone,
    string Email,
    string DocumentNumber) : ICommand<Guid>;
