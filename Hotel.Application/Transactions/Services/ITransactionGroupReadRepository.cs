using Hotel.Application.Transactions.TransferObjects;

namespace Hotel.Application.Transactions.Services;

public interface ITransactionGroupReadRepository
{
    Task<IReadOnlyCollection<TransactionGroupListDto>> GetAll(bool? isActive, CancellationToken cancellationToken);

    Task<TransactionGroupDto> GetById(Guid id, CancellationToken cancellationToken);
}
