using Hotel.Domain.Transactions.Entities;
using Hotel.Domain.Transactions.Enums;
using Hotel.Domain.Transactions.Exceptions;

namespace Hotel.Domain.Transactions.Services;

public class TransactionGroupCreationService(ITransactionGroupRepository transactionGroupRepository)
    : ITransactionGroupCreationService
{
    public async Task<TransactionGroup> CreateTransactionGroup(
        string code,
        string name,
        TransactionType type,
        CancellationToken cancellationToken = default)
    {
        var transactionGroup = TransactionGroup.Create(code, name, type);

        if (await transactionGroupRepository.ExistsByCode(transactionGroup.Code, cancellationToken))
        {
            throw new TransactionGroupCodeAlreadyExistsException(transactionGroup.Code);
        }

        await transactionGroupRepository.Add(transactionGroup, cancellationToken);

        return transactionGroup;
    }
}
