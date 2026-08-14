using Hotel.Shared.Exceptions;

namespace Hotel.Domain.FiscalAccounting.Exceptions;

public class InvalidFolioItemQuantityException()
    : DomainException("Folio item quantity must be greater than zero.")
{
}