using Hotel.Application.Orders.TransferObjects;
using MediatR;

namespace Hotel.Application.Orders.Queries;

public record GetOrderByIdQuery(Guid OrderId) : IRequest<OrderDto>;
