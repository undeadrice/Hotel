using Hotel.Application.Orders.Mapping;
using Hotel.Application.Orders.TransferObjects;
using Hotel.Domain.Orders.Services;
using MediatR;

namespace Hotel.Application.Orders.Queries;

public class GetOrdersLegacyQueryHandler(IOrderRepository orderRepository)
    : IRequestHandler<GetOrdersLegacyQuery, IReadOnlyCollection<OrderDto>>
{
    public async Task<IReadOnlyCollection<OrderDto>> Handle(GetOrdersLegacyQuery request, CancellationToken cancellationToken)
    {
        var orders = await orderRepository.GetAll(cancellationToken, x => true);
        return orders.Select(x => x.MapToOrderDto()).ToList();
    }
}
