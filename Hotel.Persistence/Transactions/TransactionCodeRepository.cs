using Hotel.Domain.Transactions.Entities;
using Hotel.Domain.Transactions.Services;
using Hotel.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;
using Hotel.Domain.Transactions.Repositories;

namespace Hotel.Persistence.Transactions;

public class TransactionCodeRepository(PersistenceDbContext dbContext) : ITransactionCodeRepository
{
    public async Task Add(TransactionCode transactionCode, CancellationToken token = default)
    {
        await dbContext.TransactionCodes.AddAsync(transactionCode, token);
    }

    public async Task<TransactionCode> GetById(Guid id, CancellationToken token = default)
    {
        var result = await dbContext.TransactionCodes.FirstOrDefaultAsync(tc => tc.Id == id, token);

        if (result is null)
        {
            throw new NotFoundException($"Transaction code with id {id} doesn't exist");
        }

        return result;
    }

    public async Task<TransactionCode?> FindById(Guid id, CancellationToken token = default)
    {
        return await dbContext.TransactionCodes.FirstOrDefaultAsync(tc => tc.Id == id, token);
    }

    public async Task<bool> ExistsByCode(string code, CancellationToken token = default)
    {
        return await dbContext.TransactionCodes.AnyAsync(tc => tc.Code == code, token);
    }
}
