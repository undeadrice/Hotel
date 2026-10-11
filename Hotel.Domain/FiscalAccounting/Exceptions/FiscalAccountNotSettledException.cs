using Hotel.Shared.Exceptions;

namespace Hotel.Domain.FiscalAccounting.Exceptions;

public class FiscalAccountNotSettledException()
    : DomainException("Fiscal account cannot be checked out because not all folios are settled.")
{
}