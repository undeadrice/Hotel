using Hotel.Application.FiscalAccounting.TransferObjects;
using MediatR;
using Hotel.Application.FiscalAccounting.Repositories;
using Hotel.Application.Pipeline;
using Hotel.Application.Users.Enums;

namespace Hotel.Application.FiscalAccounting.Queries;

[CheckPermission(Permission.FiscalAccountView)]
public record GetFiscalAccountByIdQuery(Guid Id) : IRequest<FiscalAccountDetailsDto>;

internal class GetFiscalAccountByIdQueryHandler(IFiscalAccountReadRepository fiscalAccountReadRepository)
    : IRequestHandler<GetFiscalAccountByIdQuery, FiscalAccountDetailsDto>
{
    public async Task<FiscalAccountDetailsDto> Handle(GetFiscalAccountByIdQuery request, CancellationToken cancellationToken)
    {
        return await fiscalAccountReadRepository.GetById(request.Id, cancellationToken);
    }
}
