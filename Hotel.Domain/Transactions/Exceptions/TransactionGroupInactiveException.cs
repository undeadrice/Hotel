using Hotel.Shared.Exceptions;

namespace Hotel.Domain.Transactions.Exceptions;

public class TransactionGroupInactiveException(Guid transactionGroupId)
    : DomainException($"Transaction group with id '{transactionGroupId}' is inactive and cannot be used.")
{
}
