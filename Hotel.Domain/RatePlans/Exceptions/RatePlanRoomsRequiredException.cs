namespace Hotel.Domain.RatePlans.Exceptions;

public class RatePlanRoomsRequiredException : Exception
{
    public RatePlanRoomsRequiredException()
        : base("Rate plan must have at least one room assigned.")
    {
    }
}