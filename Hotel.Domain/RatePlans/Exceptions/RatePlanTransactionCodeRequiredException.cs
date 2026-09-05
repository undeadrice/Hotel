namespace Hotel.Domain.RatePlans.Exceptions;

public class RatePlanTransactionCodeRequiredException : Exception
{
    public RatePlanTransactionCodeRequiredException()
        : base("Rate plan transaction code is required.")
    {
    }
}