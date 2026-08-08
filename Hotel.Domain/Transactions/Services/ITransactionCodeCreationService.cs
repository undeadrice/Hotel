using Hotel.Domain.Transactions.Entities;

namespace Hotel.Domain.Transactions.Services;

public interface ITransactionCodeCreationService
{
    Task<TransactionCode> CreateTransactionCode(
        Guid transactionGroupId,
        string code,
        string name,
        string? description,
        decimal defaultAmount,
        CancellationToken cancellationToken = default);
}
