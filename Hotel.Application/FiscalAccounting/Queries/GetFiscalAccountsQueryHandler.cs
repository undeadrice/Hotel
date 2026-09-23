using Hotel.Application.FiscalAccounting.TransferObjects;
using MediatR;
using Hotel.Application.FiscalAccounting.Repositories;
using Hotel.SharedPipeline;
using Hotel.SharedPipeline.Attributes;
using Hotel.Shared.Application.Users.Enums;

namespace Hotel.Application.FiscalAccounting.Queries;

[CheckPermission(Permission.FiscalAccountView)]
public record GetFiscalAccountsQuery : IRequest<IReadOnlyCollection<FiscalAccountListItemDto>>;

internal class GetFiscalAccountsQueryHandler(IFiscalAccountReadRepository fiscalAccountReadRepository)
    : IRequestHandler<GetFiscalAccountsQuery, IReadOnlyCollection<FiscalAccountListItemDto>>
{
    public async Task<IReadOnlyCollection<FiscalAccountListItemDto>> Handle(GetFiscalAccountsQuery request, CancellationToken cancellationToken)
    {
        return await fiscalAccountReadRepository.GetAll(cancellationToken);
    }
}
