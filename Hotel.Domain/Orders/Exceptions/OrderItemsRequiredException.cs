using Hotel.Shared.Exceptions;

namespace Hotel.Domain.Orders.Exceptions;

public class OrderItemsRequiredException : DomainException
{
    public OrderItemsRequiredException()
        : base("Order must contain at least one item")
    {
    }
}
