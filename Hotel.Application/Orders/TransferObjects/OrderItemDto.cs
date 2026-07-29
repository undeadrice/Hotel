namespace Hotel.Application.Orders.TransferObjects;

public record OrderItemDto(Guid ProductId, int Quantity, decimal UnitPrice);
