using Hotel.Domain.Transactions.Entities;
using Hotel.Domain.Transactions.Enums;

namespace Hotel.Domain.Transactions.Services;

public interface ITransactionGroupCreationService
{
    Task<TransactionGroup> CreateTransactionGroup(
        string code,
        string name,
        string? description,
        TransactionType type,
        CancellationToken cancellationToken = default);
}
