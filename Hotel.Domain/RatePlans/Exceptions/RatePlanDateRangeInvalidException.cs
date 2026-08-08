namespace Hotel.Domain.RatePlans.Exceptions;

public class RatePlanDateRangeInvalidException : Exception
{
    public RatePlanDateRangeInvalidException()
        : base("Rate plan end date must be after start date.")
    {
    }
}