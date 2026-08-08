using Hotel.Domain.Transactions.Entities;
using System.Linq.Expressions;

namespace Hotel.Domain.Transactions.Services;

public interface ITransactionGroupRepository
{
    Task Add(TransactionGroup transactionGroup, CancellationToken token = default);

    Task Update(TransactionGroup transactionGroup, CancellationToken token = default);

    Task<TransactionGroup> GetById(Guid id, CancellationToken token = default);

    Task<TransactionGroup?> FindById(Guid id, CancellationToken token = default);

    Task<bool> ExistsByCode(string code, CancellationToken token = default);

    Task<IReadOnlyCollection<TransactionGroup>> GetAll(CancellationToken token, Expression<Func<TransactionGroup, bool>>? filter = null);
}
