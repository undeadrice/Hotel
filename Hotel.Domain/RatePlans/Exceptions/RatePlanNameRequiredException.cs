using Hotel.Shared.Exceptions;

namespace Hotel.Domain.RatePlans.Exceptions;

public class RatePlanNameRequiredException()
    : DomainException("Rate plan name is required.")
{
}