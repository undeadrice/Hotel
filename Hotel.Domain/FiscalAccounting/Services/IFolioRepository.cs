using Hotel.Domain.FiscalAccounting.Entities;

namespace Hotel.Domain.FiscalAccounting.Services;

public interface IFolioRepository
{
    Task<Folio> GetById(Guid id, CancellationToken token = default);
}