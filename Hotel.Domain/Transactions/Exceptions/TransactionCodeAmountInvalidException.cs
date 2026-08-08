using Hotel.Shared.Exceptions;

namespace Hotel.Domain.Transactions.Exceptions;

public class TransactionCodeAmountInvalidException()
    : DomainException("Transaction code default amount cannot be negative.")
{
}
