using Hotel.API.Orders.Responses;
using Hotel.Application.Orders.TransferObjects;

namespace Hotel.API.Orders.Mappings;

public static class OrderMappingExtensions
{
    public static OrderResponse MapToOrderResponse(this OrderDto dto) =>
        new OrderResponse(
            dto.Id,
            dto.CustomerId,
            dto.Items.Select(i => new OrderItemResponse(i.ProductId, i.Quantity, i.UnitPrice)).ToList(),
            dto.FinalPrice,
            dto.CreatedAt);
}
