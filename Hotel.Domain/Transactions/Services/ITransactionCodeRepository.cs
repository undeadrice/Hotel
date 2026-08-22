using Hotel.Domain.Transactions.Entities;

namespace Hotel.Domain.Transactions.Services;

public interface ITransactionCodeRepository
{
    Task Add(TransactionCode transactionCode, CancellationToken token = default);

    Task<TransactionCode> GetById(Guid id, CancellationToken token = default);

    Task<TransactionCode?> FindById(Guid id, CancellationToken token = default);

    Task<bool> ExistsByCode(string code, CancellationToken token = default);
}
