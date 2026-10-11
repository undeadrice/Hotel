using Hotel.Shared.Exceptions;

namespace Hotel.Domain.Transactions.Exceptions;

public class TransactionGroupNameRequiredException()
    : DomainException("Transaction group name is required.")
{
}
