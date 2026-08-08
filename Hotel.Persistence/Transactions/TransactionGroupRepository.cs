using Hotel.Domain.Transactions.Entities;
using Hotel.Domain.Transactions.Services;
using Hotel.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Hotel.Persistence.Transactions;

public class TransactionGroupRepository(PersistenceDbContext dbContext) : ITransactionGroupRepository
{
    public async Task Add(TransactionGroup transactionGroup, CancellationToken token = default)
    {
        await dbContext.TransactionGroups.AddAsync(transactionGroup, token);
    }

    public Task Update(TransactionGroup transactionGroup, CancellationToken token = default)
    {
        dbContext.TransactionGroups.Update(transactionGroup);
        return Task.CompletedTask;
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

    public async Task<IReadOnlyCollection<TransactionGroup>> GetAll(
        CancellationToken token,
        Expression<Func<TransactionGroup, bool>>? filter = null)
    {
        if (filter is null)
        {
            return await dbContext.TransactionGroups.ToListAsync(token);
        }

        return await dbContext.TransactionGroups.Where(filter).ToListAsync(token);
    }
}
