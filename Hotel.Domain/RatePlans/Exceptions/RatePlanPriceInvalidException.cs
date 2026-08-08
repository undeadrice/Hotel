namespace Hotel.Domain.RatePlans.Exceptions;

public class RatePlanPriceInvalidException : Exception
{
    public RatePlanPriceInvalidException()
        : base("Rate plan price must be greater than zero.")
    {
    }
}