using Hotel.Shared.Exceptions;

namespace Hotel.Domain.RatePlans.Exceptions;

public class RatePlanTransactionCodeRequiredException()
    : DomainException("Rate plan transaction code is required.")
{
}