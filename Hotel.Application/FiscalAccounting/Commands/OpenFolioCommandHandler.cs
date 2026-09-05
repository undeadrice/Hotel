using Hotel.Application.Common;
using MediatR;
using Hotel.Domain.FiscalAccounting.Repositories;
using Hotel.Application.Pipeline;
using Hotel.Application.Users.Enums;

namespace Hotel.Application.FiscalAccounting.Commands;

[CheckPermission(Permission.FiscalAccountEdit)]
public record OpenFolioCommand(Guid FiscalAccountId) : ICommand<Guid>;

internal class OpenFolioCommandHandler(
    IFiscalAccountRepository fiscalAccountRepository,
    IDateTimeProvider dateTimeProvider)
    : IRequestHandler<OpenFolioCommand, Guid>
{
    public async Task<Guid> Handle(OpenFolioCommand request, CancellationToken cancellationToken)
    {
        var account = await fiscalAccountRepository.GetById(request.FiscalAccountId, cancellationToken);

        var folio = account.OpenFolio(dateTimeProvider.UtcNow);

        return folio.Id;
    }
}
