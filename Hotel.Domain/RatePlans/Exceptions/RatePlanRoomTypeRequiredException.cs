using Hotel.Shared.Exceptions;

namespace Hotel.Domain.RatePlans.Exceptions;

public class RatePlanRoomTypeRequiredException()
    : DomainException("Rate plan room type is required.")
{
}