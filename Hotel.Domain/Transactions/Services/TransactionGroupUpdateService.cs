using Hotel.Domain.Transactions.Enums;
using Hotel.Domain.Transactions.Exceptions;

namespace Hotel.Domain.Transactions.Services;

public class TransactionGroupUpdateService(ITransactionGroupRepository transactionGroupRepository)
    : ITransactionGroupUpdateService
{
    public async Task UpdateTransactionGroup(
        Guid id,
        string code,
        string name,
        TransactionType type,
        CancellationToken cancellationToken = default)
    {
        var transactionGroup = await transactionGroupRepository.GetById(id, cancellationToken);

        var normalizedCode = code.Trim().ToUpperInvariant();

        if (transactionGroup.Code != normalizedCode
            && !string.IsNullOrWhiteSpace(normalizedCode)
            && await transactionGroupRepository.ExistsByCode(normalizedCode, cancellationToken))
        {
            throw new TransactionGroupCodeAlreadyExistsException(normalizedCode);
        }

        transactionGroup.Update(code, name, type);
    }
}
