using Hotel.Application.FiscalAccounting.Services;
using Hotel.Application.FiscalAccounting.TransferObjects;
using MediatR;

namespace Hotel.Application.FiscalAccounting.Queries;

internal class GetFiscalAccountByIdQueryHandler(IFiscalAccountReadRepository fiscalAccountReadRepository)
    : IRequestHandler<GetFiscalAccountByIdQuery, FiscalAccountDetailsDto>
{
    public async Task<FiscalAccountDetailsDto> Handle(GetFiscalAccountByIdQuery request, CancellationToken cancellationToken)
    {
        return await fiscalAccountReadRepository.GetById(request.Id, cancellationToken);
    }
}