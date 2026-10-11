using Hotel.Shared.Exceptions;

namespace Hotel.Domain.Reservations.Exceptions;

public class RatePlanInvalidForRoomException(string message) : DomainException(message)
{
    public static RatePlanInvalidForRoomException RoomNotInRatePlan()
        => new("The selected room is not included in this rate plan.");

    public static RatePlanInvalidForRoomException DateRangeMismatch()
        => new("The reservation dates must be within the rate plan's validity period.");
}