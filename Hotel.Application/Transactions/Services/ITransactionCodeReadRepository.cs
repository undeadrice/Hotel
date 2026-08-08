using Hotel.Application.Transactions.TransferObjects;

namespace Hotel.Application.Transactions.Services;

public interface ITransactionCodeReadRepository
{
    Task<IReadOnlyCollection<TransactionCodeListDto>> GetAll(
        Guid? transactionGroupId,
        bool? isActive,
        CancellationToken cancellationToken);

    Task<TransactionCodeDto> GetById(Guid id, CancellationToken cancellationToken);
}
