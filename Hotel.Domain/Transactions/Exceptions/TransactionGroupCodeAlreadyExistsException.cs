using Hotel.Shared.Exceptions;

namespace Hotel.Domain.Transactions.Exceptions;

public class TransactionGroupCodeAlreadyExistsException(string code)
    : DomainException($"Transaction group with code '{code}' already exists.")
{
}
