
using Hotel.Shared.Exceptions;

namespace Hotel.Domain.Products.Exceptions;

public class ProductDescriptionRequiredException() : DomainException("Product description is required.")
{ 
}