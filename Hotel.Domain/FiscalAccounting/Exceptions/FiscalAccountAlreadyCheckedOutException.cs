using Hotel.Shared.Exceptions;

namespace Hotel.Domain.FiscalAccounting.Exceptions;

public class FiscalAccountAlreadyCheckedOutException()
    : DomainException("Fiscal account has already been checked out.")
{
}