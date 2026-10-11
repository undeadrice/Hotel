using Hotel.Shared.Exceptions;

namespace Hotel.Domain.NumberCycles.Exceptions;

public class NumberCyclePrefixRequiredException() : DomainException("Prefix is required.")
{
}