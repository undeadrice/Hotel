using Hotel.Shared.Exceptions;

namespace Hotel.Domain.FiscalAccounting.Exceptions;

public class InvalidFolioItemAmountException()
    : DomainException("Folio item amount cannot be negative.")
{
}