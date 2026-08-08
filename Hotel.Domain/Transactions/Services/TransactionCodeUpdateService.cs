using Hotel.Domain.Transactions.Exceptions;

namespace Hotel.Domain.Transactions.Services;

public class TransactionCodeUpdateService(
    ITransactionCodeRepository transactionCodeRepository,
    ITransactionGroupRepository transactionGroupRepository)
    : ITransactionCodeUpdateService
{
    public async Task UpdateTransactionCode(
        Guid id,
        Guid transactionGroupId,
        string code,
        string name,
        string? description,
        decimal defaultAmount,
        CancellationToken cancellationToken = default)
    {
        var transactionCode = await transactionCodeRepository.GetById(id, cancellationToken);

        if (transactionCode.TransactionGroupId != transactionGroupId)
        {
            var transactionGroup = await transactionGroupRepository.GetById(transactionGroupId, cancellationToken);

            if (!transactionGroup.IsActive)
            {
                throw new TransactionGroupInactiveException(transactionGroupId);
            }

            transactionCode.ChangeGroup(transactionGroupId);
        }

        var normalizedCode = code?.Trim().ToUpperInvariant();

        if (transactionCode.Code != normalizedCode
            && !string.IsNullOrWhiteSpace(normalizedCode)
            && await transactionCodeRepository.ExistsByCode(normalizedCode, cancellationToken))
        {
            throw new TransactionCodeAlreadyExistsException(normalizedCode);
        }

        transactionCode.Update(code!, name, description, defaultAmount);

        await transactionCodeRepository.Update(transactionCode, cancellationToken);
    }
}
