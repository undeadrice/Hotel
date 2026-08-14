using Hotel.Application.Configurations.Services;
using Hotel.Domain.FiscalAccounting.Services;
using MediatR;

namespace Hotel.Application.FiscalAccounting.Commands;

public class CreateFolioItemCommandHandler(
    IFolioRepository folioRepository,
    IBusinessDateProvider businessDateProvider)
    : IRequestHandler<CreateFolioItemCommand, Guid>
{
    public async Task<Guid> Handle(CreateFolioItemCommand request, CancellationToken cancellationToken)
    {
        var folio = await folioRepository.GetById(request.FolioId, cancellationToken);

        var businessDate = await businessDateProvider.GetCurrentBusinessDate(cancellationToken);

        var item = folio.AddItem(
            request.Description,
            request.Quantity,
            request.Amount,
            request.TransactionCodeId,
            businessDate);

        return item.Id;
    }
}