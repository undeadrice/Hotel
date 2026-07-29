using Hotel.Shared.Exceptions;

namespace Hotel.Domain.Products.Exceptions;

public class ProductPriceInvalidException() : DomainException("Product price must be greater than zero.")
{ 
}