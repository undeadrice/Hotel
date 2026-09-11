namespace Hotel.Domain.RatePlans.Exceptions;

public class RatePlanNameRequiredException : Exception
{
    public RatePlanNameRequiredException()
        : base("Rate plan name is required.")
    {
    }
}