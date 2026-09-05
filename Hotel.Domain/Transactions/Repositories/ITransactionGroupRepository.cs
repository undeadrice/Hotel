using Hotel.Domain.Transactions.Entities;

namespace Hotel.Domain.Transactions.Repositories;

public interface ITransactionGroupRepository
{
    Task Add(TransactionGroup transactionGroup, CancellationToken token = default);

    Task<TransactionGroup> GetById(Guid id, CancellationToken token = default);

    Task<TransactionGroup?> FindById(Guid id, CancellationToken token = default);

    Task<bool> ExistsByCode(string code, CancellationToken token = default);
}
