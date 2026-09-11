using Hotel.Shared.Exceptions;

namespace Hotel.Domain.Transactions.Exceptions;

public class TransactionCodeCodeRequiredException()
    : DomainException("Transaction code is required.")
{
}
