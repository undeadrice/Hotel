using Hotel.Shared.Exceptions;

namespace Hotel.Domain.Transactions.Exceptions;

public class TransactionGroupCodeRequiredException()
    : DomainException("Transaction group code is required.")
{
}
