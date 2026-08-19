using Hotel.Domain.FiscalAccounting.Entities;

namespace Hotel.Domain.FiscalAccounting.Services;

public interface IFiscalAccountRepository
{
    Task Add(FiscalAccount account, CancellationToken token = default);

    Task Update(FiscalAccount account, CancellationToken token = default);

    Task<FiscalAccount?> FindById(Guid id, CancellationToken token = default);

    Task<FiscalAccount> GetById(Guid id, CancellationToken token = default);

    Task<FiscalAccount> GetByFolioId(Guid folioId, CancellationToken token = default);

    Task<FiscalAccount> GetForSettlement(Guid accountId, Guid folioId, CancellationToken token = default);

    Task<FiscalAccount> GetForCheckOut(Guid accountId, CancellationToken token = default);
}
