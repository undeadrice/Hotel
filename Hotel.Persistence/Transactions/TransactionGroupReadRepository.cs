using Hotel.Application.Transactions.TransferObjects;
using Hotel.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;
using Hotel.Application.Transactions.Repositories;

namespace Hotel.Persistence.Transactions;

public class TransactionGroupReadRepository(PersistenceDbContext dbContext) : ITransactionGroupReadRepository
{
    public async Task<IReadOnlyCollection<TransactionGroupListDto>> GetAll(
        bool? isActive,
        CancellationToken cancellationToken)
    {
        var query = dbContext.TransactionGroups.AsNoTracking();

        if (isActive.HasValue)
        {
            query = query.Where(tg => tg.IsActive == isActive.Value);
        }

        return await query
            .OrderBy(tg => tg.Code)
            .Select(tg => new TransactionGroupListDto(
                tg.Id,
                tg.Code,
                tg.Name,
                tg.Type,
                tg.IsActive,
                tg.TransactionCodes.Count))
            .ToListAsync(cancellationToken);
    }

    public async Task<TransactionGroupDto> GetById(Guid id, CancellationToken cancellationToken)
    {
        var transactionGroup = await dbContext.TransactionGroups
            .AsNoTracking()
            .Where(tg => tg.Id == id)
            .Select(tg => new TransactionGroupDto(
                tg.Id,
                tg.Code,
                tg.Name,
                tg.Type,
                tg.IsActive,
                tg.TransactionCodes
                    .OrderBy(tc => tc.Code)
                    .Select(tc => new TransactionCodeListDto(
                        tc.Id,
                        tc.TransactionGroupId,
                        tg.Name,
                        tc.Code,
                        tc.Name,
                        tc.IsActive))
                    .ToList()))
            .FirstOrDefaultAsync(cancellationToken);

        if (transactionGroup is null)
        {
            throw new NotFoundException($"Transaction group with id {id} doesn't exist");
        }

        return transactionGroup;
    }
}
