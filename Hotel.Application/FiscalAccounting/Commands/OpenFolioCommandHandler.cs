using Hotel.Application.Common;
using MediatR;
using Hotel.Domain.FiscalAccounting.Repositories;

namespace Hotel.Application.FiscalAccounting.Commands;

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
