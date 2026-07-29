using Hotel.Application.Orders.TransferObjects;
using MediatR;

namespace Hotel.Application.Orders.Queries;

public record GetOrdersLegacyQuery() : IRequest<IReadOnlyCollection<OrderDto>>;
