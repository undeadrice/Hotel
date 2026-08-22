using Hotel.Domain.Transactions.Entities;
using Hotel.Domain.Transactions.Services;
using Hotel.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Persistence.Transactions;

public class TransactionGroupRepository(PersistenceDbContext dbContext) : ITransactionGroupRepository
{
    public async Task Add(TransactionGroup transactionGroup, CancellationToken token = default)
    {
        await dbContext.TransactionGroups.AddAsync(transactionGroup, token);
    }

    public async Task<TransactionGroup> GetById(Guid id, CancellationToken token = default)
    {
        var result = await dbContext.TransactionGroups.FirstOrDefaultAsync(tg => tg.Id == id, token);

        if (result is null)
        {
            throw new NotFoundException($"Transaction group with id {id} doesn't exist");
        }

        return result;
    }

    public async Task<TransactionGroup?> FindById(Guid id, CancellationToken token = default)
    {
        return await dbContext.TransactionGroups.FirstOrDefaultAsync(tg => tg.Id == id, token);
    }

    public async Task<bool> ExistsByCode(string code, CancellationToken token = default)
    {
        return await dbContext.TransactionGroups.AnyAsync(tg => tg.Code == code, token);
    }
}
