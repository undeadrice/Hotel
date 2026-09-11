using Hotel.Shared.Exceptions;

namespace Hotel.Domain.FiscalAccounting.Exceptions;

public class FiscalAccountCycleIdentifierRequiredException() : DomainException("Cycle identifier is required.")
{
}