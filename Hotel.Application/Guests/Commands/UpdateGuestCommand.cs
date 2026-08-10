using Hotel.Application.Pipeline;
using Hotel.Application.Users.Enums;

namespace Hotel.Application.Guests.Commands;

[CheckPermission(Permission.GuestEdit)]
public record UpdateGuestCommand(
    Guid Id,
    string FirstName,
    string LastName,
    string Phone,
    string Email,
    string DocumentNumber) : ICommand;
