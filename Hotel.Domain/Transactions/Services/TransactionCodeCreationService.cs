using Hotel.Domain.Transactions.Entities;
using Hotel.Domain.Transactions.Exceptions;

namespace Hotel.Domain.Transactions.Services;

public class TransactionCodeCreationService(
    ITransactionCodeRepository transactionCodeRepository,
    ITransactionGroupRepository transactionGroupRepository)
    : ITransactionCodeCreationService
{
    public async Task<TransactionCode> CreateTransactionCode(
        Guid transactionGroupId,
        string code,
        string name,
        string? description,
        decimal defaultAmount,
        CancellationToken cancellationToken = default)
    {
        var transactionGroup = await transactionGroupRepository.GetById(transactionGroupId, cancellationToken);

        if (!transactionGroup.IsActive)
        {
            throw new TransactionGroupInactiveException(transactionGroupId);
        }

        var transactionCode = TransactionCode.Create(transactionGroupId, code, name, description, defaultAmount);

        if (await transactionCodeRepository.ExistsByCode(transactionCode.Code, cancellationToken))
        {
            throw new TransactionCodeAlreadyExistsException(transactionCode.Code);
        }

        await transactionCodeRepository.Add(transactionCode, cancellationToken);

        return transactionCode;
    }
}
