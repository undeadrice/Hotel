using Hotel.Shared.Exceptions;

namespace Hotel.Domain.FiscalAccounting.Exceptions;

public class FolioAlreadySettledException()
    : DomainException("Folio has already been settled.")
{
}