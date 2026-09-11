using Hotel.Shared.Exceptions;

namespace Hotel.Domain.NumberCycles.Exceptions;

public class NumberCycleInvalidTopicException() : DomainException("Invalid number cycle topic.")
{
}