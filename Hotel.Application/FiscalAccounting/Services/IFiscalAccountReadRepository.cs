using Hotel.Application.FiscalAccounting.TransferObjects;

namespace Hotel.Application.FiscalAccounting.Services;

public interface IFiscalAccountReadRepository
{
    Task<IReadOnlyCollection<FiscalAccountListItemDto>> GetAll(CancellationToken cancellationToken);

    Task<FiscalAccountDetailsDto> GetById(Guid id, CancellationToken cancellationToken);
}