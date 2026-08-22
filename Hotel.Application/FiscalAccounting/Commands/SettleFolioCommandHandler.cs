using MediatR;
using Hotel.Domain.FiscalAccounting.Repositories;

namespace Hotel.Application.FiscalAccounting.Commands;

public class SettleFolioCommandHandler(IFiscalAccountRepository fiscalAccountRepository)
    : IRequestHandler<SettleFolioCommand>
{
    public async Task Handle(SettleFolioCommand request, CancellationToken cancellationToken)
    {
        var account = await fiscalAccountRepository.GetForSettlement(
            request.AccountId,
            request.FolioId,
            cancellationToken);

        account.SettleFolio(request.FolioId);
    }
}