using Hotel.Shared.Exceptions;

namespace Hotel.Domain.FiscalAccounting.Exceptions;

public class FolioNotFoundException(Guid folioId)
    : DomainException($"Folio with id {folioId} doesn't exist.")
{
}