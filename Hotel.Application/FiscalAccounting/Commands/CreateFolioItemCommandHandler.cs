using Hotel.Application.Configurations.Services;
using Hotel.Domain.FiscalAccounting.Services;
using Hotel.Domain.Transactions.Services;
using MediatR;

namespace Hotel.Application.FiscalAccounting.Commands;

public class CreateFolioItemCommandHandler(
    IFiscalAccountRepository fiscalAccountRepository,
    IBusinessDateProvider businessDateProvider,
    ITransactionCodeRepository transactionCodeRepository,
    ITransactionGroupRepository transactionGroupRepository)
    : IRequestHandler<CreateFolioItemCommand, Guid>
{
    public async Task<Guid> Handle(CreateFolioItemCommand request, CancellationToken cancellationToken)
    {
        var account = await fiscalAccountRepository.GetByFolioId(request.FolioId, cancellationToken);

        var transactionCode = await transactionCodeRepository.GetById(request.TransactionCodeId, cancellationToken);
        var transactionGroup = await transactionGroupRepository.GetById(transactionCode.TransactionGroupId, cancellationToken);

        var businessDate = await businessDateProvider.GetCurrentBusinessDate(cancellationToken);

        var item = account.AddFolioItem(
            request.FolioId,
            request.Description,
            request.Quantity,
            request.Amount,
            request.TransactionCodeId,
            transactionGroup.Type,
            businessDate);

        return item.Id;
    }
}