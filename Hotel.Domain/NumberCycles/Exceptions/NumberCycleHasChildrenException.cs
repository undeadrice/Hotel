using Hotel.Domain.NumberCycles.Enums;
using Hotel.Shared.Exceptions;

namespace Hotel.Domain.NumberCycles.Exceptions;

public class NumberCycleHasChildrenException(NumberCycleTopic topic)
    : DomainException($"Number cycle for topic '{topic}' cannot be deleted because related records exist.")
{
}