using Hotel.Domain.Transactions.Enums;

namespace Hotel.Domain.Transactions.Services;

public interface ITransactionGroupUpdateService
{
    Task UpdateTransactionGroup(
        Guid id,
        string code,
        string name,
        string? description,
        TransactionType type,
        CancellationToken cancellationToken = default);
}
