using Hotel.Shared.Exceptions;

namespace Hotel.Domain.FiscalAccounting.Exceptions;

public class FolioNotBalancedException()
    : DomainException("Folio cannot be settled because charges and payments do not balance.")
{
}