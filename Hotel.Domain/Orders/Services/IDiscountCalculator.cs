using Hotel.Domain.Customers;

namespace Hotel.Domain.Orders.Services;

public record OrderLineItem(decimal UnitPrice, int Quantity);

public interface IDiscountCalculator
{
    decimal CalculateDiscount(IEnumerable<OrderLineItem> items, CustomerLocation location, DateTime orderDate);
}
