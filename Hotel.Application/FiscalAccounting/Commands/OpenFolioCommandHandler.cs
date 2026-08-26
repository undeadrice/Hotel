using MediatR;
using Hotel.Domain.FiscalAccounting.Repositories;

namespace Hotel.Application.FiscalAccounting.Commands;

internal class OpenFolioCommandHandler(IFiscalAccountRepository fiscalAccountRepository)
    : IRequestHandler<OpenFolioCommand, Guid>
{
    public async Task<Guid> Handle(OpenFolioCommand request, CancellationToken cancellationToken)
    {
        var account = await fiscalAccountRepository.GetById(request.FiscalAccountId, cancellationToken);

        var folio = account.OpenFolio();

        return folio.Id;
    }
}