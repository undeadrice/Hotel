using Hotel.Application.Pipeline;
using Hotel.Application.Users.Enums;

namespace Hotel.Application.Customers.Commands;

[CheckRole(UserRole.Admin)]
public record UpdateCustomerCommand(
    Guid Id,
    string FirstName,
    string LastName,
    string Phone,
    string Email,
    string DocumentNumber) : ICommand;
