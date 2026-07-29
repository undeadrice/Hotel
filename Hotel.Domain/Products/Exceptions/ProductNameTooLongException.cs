using Hotel.Shared.Exceptions;

namespace Hotel.Domain.Products.Exceptions;

public class ProductNameTooLongException() : DomainException("Product name cannot exceed 50 characters.")
{
}