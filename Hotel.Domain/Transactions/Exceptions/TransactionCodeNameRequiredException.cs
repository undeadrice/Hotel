using Hotel.Shared.Exceptions;

namespace Hotel.Domain.Transactions.Exceptions;

public class TransactionCodeNameRequiredException()
    : DomainException("Transaction code name is required.")
{
}
