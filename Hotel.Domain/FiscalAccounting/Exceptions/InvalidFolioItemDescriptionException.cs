using Hotel.Shared.Exceptions;

namespace Hotel.Domain.FiscalAccounting.Exceptions;

public class InvalidFolioItemDescriptionException()
    : DomainException("Folio item description is required.")
{
}