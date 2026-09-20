namespace Hotel.Domain.Transactions.Services;

public interface ITransactionCodeUpdateService
{
    Task UpdateTransactionCode(
        Guid id,
        Guid transactionGroupId,
        string code,
        string name,
        CancellationToken cancellationToken = default);
}
