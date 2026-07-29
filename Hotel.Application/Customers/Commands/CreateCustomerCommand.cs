using Hotel.Application.Pipeline;
using Hotel.Application.Users.Enums;
using Hotel.Domain.Customers;

namespace Hotel.Application.Customers.Commands;

[CheckRole(UserRole.Admin)]
public record CreateCustomerCommand(CustomerLocation Location) : ICommand<Guid>;
