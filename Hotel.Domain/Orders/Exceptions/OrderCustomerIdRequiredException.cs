using Hotel.Shared.Exceptions;

namespace Hotel.Domain.Orders.Exceptions;

public class OrderCustomerIdRequiredException : DomainException
{
    public OrderCustomerIdRequiredException()
        : base("Order customer id is required")
    {
    }
}
