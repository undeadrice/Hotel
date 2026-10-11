using Hotel.Shared.Exceptions;

namespace Hotel.Domain.RatePlans.Exceptions;

public class RatePlanRoomsRequiredException()
    : DomainException("Rate plan must have at least one room assigned.")
{
}