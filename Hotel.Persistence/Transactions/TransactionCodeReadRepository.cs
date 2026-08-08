using Hotel.Application.Transactions.Services;
using Hotel.Application.Transactions.TransferObjects;
using Hotel.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Persistence.Transactions;

public class TransactionCodeReadRepository(PersistenceDbContext dbContext) : ITransactionCodeReadRepository
{
    public async Task<IReadOnlyCollection<TransactionCodeListDto>> GetAll(
        Guid? transactionGroupId,
        bool? isActive,
        CancellationToken cancellationToken)
    {
        var query = dbContext.TransactionCodes.AsNoTracking();

        if (transactionGroupId.HasValue)
        {
            query = query.Where(tc => tc.TransactionGroupId == transactionGroupId.Value);
        }

        if (isActive.HasValue)
        {
            query = query.Where(tc => tc.IsActive == isActive.Value);
        }

        return await query
            .OrderBy(tc => tc.Code)
            .Select(tc => new TransactionCodeListDto(
                tc.Id,
                tc.TransactionGroupId,
                tc.TransactionGroup!.Name,
                tc.Code,
                tc.Name,
                tc.DefaultAmount,
                tc.IsActive))
            .ToListAsync(cancellationToken);
    }

    public async Task<TransactionCodeDto> GetById(Guid id, CancellationToken cancellationToken)
    {
        var transactionCode = await dbContext.TransactionCodes
            .AsNoTracking()
            .Where(tc => tc.Id == id)
            .Select(tc => new TransactionCodeDto(
                tc.Id,
                tc.TransactionGroupId,
                tc.TransactionGroup!.Name,
                tc.Code,
                tc.Name,
                tc.Description,
                tc.DefaultAmount,
                tc.IsActive))
            .FirstOrDefaultAsync(cancellationToken);

        if (transactionCode is null)
        {
            throw new NotFoundException($"Transaction code with id {id} doesn't exist");
        }

        return transactionCode;
    }
}
