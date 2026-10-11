using Hotel.Application.Common;
using Hotel.Application.Configurations.Services;
using Hotel.Domain.FiscalAccounting.Enums;
using Hotel.Domain.Transactions.Enums;
using MediatR;
using Hotel.Domain.Transactions.Repositories;
using Hotel.Domain.FiscalAccounting.Repositories;
using Hotel.Application.Pipeline;
using Hotel.Application.Users.Enums;

namespace Hotel.Application.FiscalAccounting.Commands;

[CheckPermission(Permission.FiscalAccountEdit)]
public record CreateFolioItemCommand(
    Guid FolioId,
    string Description,
    int Quantity,
    decimal Amount,
    Guid TransactionCodeId)
    : ICommand<Guid>;

internal class CreateFolioItemCommandHandler(
    IFiscalAccountRepository fiscalAccountRepository,
    IBusinessDateProvider businessDateProvider,
    ITransactionCodeRepository transactionCodeRepository,
    ITransactionGroupRepository transactionGroupRepository,
    IDateTimeProvider dateTimeProvider)
    : IRequestHandler<CreateFolioItemCommand, Guid>
{
    public async Task<Guid> Handle(CreateFolioItemCommand request, CancellationToken cancellationToken)
    {
        var account = await fiscalAccountRepository.GetByFolioId(request.FolioId, cancellationToken);

        var transactionCode = await transactionCodeRepository.GetById(request.TransactionCodeId, cancellationToken);
        var transactionGroup = await transactionGroupRepository.GetById(transactionCode.TransactionGroupId, cancellationToken);

        var businessDate = await businessDateProvider.GetCurrentBusinessDate(cancellationToken);

        var itemType = transactionGroup.Type == TransactionType.Charge
            ? FolioItemType.Charge
            : FolioItemType.Payment;

        var item = account.AddFolioItem(
            request.FolioId,
            request.Description,
            request.Quantity,
            request.Amount,
            request.TransactionCodeId,
            itemType,
            businessDate,
            dateTimeProvider.UtcNow);

        return item.Id;
    }
}
