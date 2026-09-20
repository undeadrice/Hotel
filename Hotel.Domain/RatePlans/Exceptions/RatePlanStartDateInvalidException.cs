using Hotel.Shared.Exceptions;

namespace Hotel.Domain.RatePlans.Exceptions;

public class RatePlanStartDateInvalidException()
    : DomainException("Rate plan start date cannot be before the current business date.")
{
}