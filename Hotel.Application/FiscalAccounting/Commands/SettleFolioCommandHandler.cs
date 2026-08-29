using MediatR;
using Hotel.Domain.FiscalAccounting.Repositories;
using Hotel.Application.Pipeline;
using Hotel.Application.Users.Enums;

namespace Hotel.Application.FiscalAccounting.Commands;

[CheckPermission(Permission.FiscalAccountEdit)]
public record SettleFolioCommand(Guid AccountId, Guid FolioId) : ICommand;

internal class SettleFolioCommandHandler(IFiscalAccountRepository fiscalAccountRepository)
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
