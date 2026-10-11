using Hotel.Domain.NumberCycles.Enums;
using Hotel.Shared.Exceptions;

namespace Hotel.Domain.NumberCycles.Exceptions;

public class NumberCycleAlreadyExistsException(NumberCycleTopic topic)
    : DomainException($"Number cycle for topic '{topic}' already exists.")
{
}