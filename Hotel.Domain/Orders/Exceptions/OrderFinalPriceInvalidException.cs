using Hotel.Shared.Exceptions;

namespace Hotel.Domain.Orders.Exceptions;

public class OrderFinalPriceInvalidException : DomainException
{
    public OrderFinalPriceInvalidException()
        : base("Order final price cannot be negative")
    {
    }
}
