using Hotel.Application.Pipeline;
using Hotel.Application.Users.Enums;

namespace Hotel.Application.Orders.Commands;

[CheckRole(UserRole.User)]
public record CreateOrderCommand(Guid CustomerId, List<OrderItemRequest> Items) : ICommand<Guid>;

public record OrderItemRequest(Guid ProductId, int Quantity);