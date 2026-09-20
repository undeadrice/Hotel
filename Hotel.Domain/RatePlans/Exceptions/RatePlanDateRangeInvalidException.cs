using Hotel.Shared.Exceptions;

namespace Hotel.Domain.RatePlans.Exceptions;

public class RatePlanDateRangeInvalidException()
    : DomainException("Rate plan end date must be after start date.")
{
}