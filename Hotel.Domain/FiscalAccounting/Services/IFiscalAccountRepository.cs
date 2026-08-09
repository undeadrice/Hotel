using Hotel.Domain.FiscalAccounting.Entities;
using System.Linq.Expressions;

namespace Hotel.Domain.FiscalAccounting.Services;

public interface IFiscalAccountRepository
{
    Task Add(FiscalAccount account, CancellationToken token = default);

    Task Update(FiscalAccount account, CancellationToken token = default);

    Task<FiscalAccount?> FindById(Guid id, CancellationToken token = default);

    Task<FiscalAccount> GetById(Guid id, CancellationToken token = default);

    Task<IReadOnlyCollection<FiscalAccount>> GetAll(CancellationToken token, Expression<Func<FiscalAccount, bool>>? filter = null);
}