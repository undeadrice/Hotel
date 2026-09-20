using Hotel.Shared.Exceptions;

namespace Hotel.Domain.NumberCycles.Exceptions;

public class NumberCycleStartIndexInvalidException() : DomainException("Start index must be zero or greater.")
{
}