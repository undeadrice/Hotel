using Hotel.Application.FiscalAccounting.Services;
using Hotel.Application.FiscalAccounting.TransferObjects;
using MediatR;

namespace Hotel.Application.FiscalAccounting.Queries;

internal class GetFiscalAccountsQueryHandler(IFiscalAccountReadRepository fiscalAccountReadRepository)
    : IRequestHandler<GetFiscalAccountsQuery, IReadOnlyCollection<FiscalAccountListItemDto>>
{
    public async Task<IReadOnlyCollection<FiscalAccountListItemDto>> Handle(GetFiscalAccountsQuery request, CancellationToken cancellationToken)
    {
        return await fiscalAccountReadRepository.GetAll(cancellationToken);
    }
}