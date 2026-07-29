using Hotel.Shared.Exceptions;

namespace Hotel.Domain.Products.Exceptions;

public class ProductStockInvalidException() : DomainException("Product stock cannot be negative.")
{ 
}