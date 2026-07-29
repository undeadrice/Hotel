using Hotel.Application.Orders.Services;
using Hotel.Application.Orders.TransferObjects;
using Hotel.Shared.Exceptions;
using MediatR;

namespace Hotel.Application.Orders.Queries;

public class GetOrderByIdQueryHandler(IOrderReadRepository orderReadRepository) : IRequestHandler<GetOrderByIdQuery, OrderDto>
{
    public async Task<OrderDto> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var order = await orderReadRepository.FindById(request.OrderId, cancellationToken);
        return order ?? throw new NotFoundException($"Order with id {request.OrderId} doesn't exist");
    }
}
