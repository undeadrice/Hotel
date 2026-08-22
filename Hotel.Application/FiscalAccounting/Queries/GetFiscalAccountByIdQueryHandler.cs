using Hotel.Application.FiscalAccounting.TransferObjects;
using MediatR;
using Hotel.Application.FiscalAccounting.Repositories;

namespace Hotel.Application.FiscalAccounting.Queries;

internal class GetFiscalAccountByIdQueryHandler(IFiscalAccountReadRepository fiscalAccountReadRepository)
    : IRequestHandler<GetFiscalAccountByIdQuery, FiscalAccountDetailsDto>
{
    public async Task<FiscalAccountDetailsDto> Handle(GetFiscalAccountByIdQuery request, CancellationToken cancellationToken)
    {
        return await fiscalAccountReadRepository.GetById(request.Id, cancellationToken);
    }
}