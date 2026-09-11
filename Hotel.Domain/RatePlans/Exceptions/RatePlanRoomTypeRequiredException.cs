namespace Hotel.Domain.RatePlans.Exceptions;

public class RatePlanRoomTypeRequiredException : Exception
{
    public RatePlanRoomTypeRequiredException()
        : base("Rate plan room type is required.")
    {
    }
}