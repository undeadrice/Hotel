using Hotel.Shared.Exceptions;

namespace Hotel.Domain.Products.Exceptions;

public class ProductDescriptionTooLongException() : DomainException("Product description cannot exceed 50 characters.")
{
}