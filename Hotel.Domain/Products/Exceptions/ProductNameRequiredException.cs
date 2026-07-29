using Hotel.Shared.Exceptions;

namespace Hotel.Domain.Products.Exceptions;

public class ProductNameRequiredException() : DomainException("Product name is required.")
{
}