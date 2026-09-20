using Hotel.Shared.Exceptions;

namespace Hotel.Domain.RatePlans.Exceptions;

public class RatePlanPriceInvalidException()
    : DomainException("Rate plan price must be positive.")
{
}