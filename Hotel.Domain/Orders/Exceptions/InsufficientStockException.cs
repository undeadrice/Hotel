using Hotel.Shared.Exceptions;

namespace Hotel.Domain.Orders.Exceptions;

public class InsufficientStockException : DomainException
{
    public InsufficientStockException(string productName, int requestedQuantity, int availableStock)
        : base($"Insufficient stock for product '{productName}'. Requested: {requestedQuantity}, Available: {availableStock}")
    {
    }
}
