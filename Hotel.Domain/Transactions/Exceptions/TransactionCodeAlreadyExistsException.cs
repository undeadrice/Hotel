using Hotel.Shared.Exceptions;

namespace Hotel.Domain.Transactions.Exceptions;

public class TransactionCodeAlreadyExistsException(string code)
    : DomainException($"Transaction code '{code}' already exists.")
{
}
