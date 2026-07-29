using Hotel.Application.Orders.TransferObjects;
using MediatR;

namespace Hotel.Application.Orders.Queries;

public record GetOrdersQuery() : IRequest<IReadOnlyCollection<OrderDto>>;