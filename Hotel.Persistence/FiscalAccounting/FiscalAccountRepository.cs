using Hotel.Domain.FiscalAccounting.Entities;
using Hotel.Domain.FiscalAccounting.Services;
using Hotel.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Hotel.Persistence.FiscalAccounting;

public class FiscalAccountRepository(PersistenceDbContext persistenceDbContext) : IFiscalAccountRepository
{
    public async Task Add(FiscalAccount account, CancellationToken token)
    {
        await persistenceDbContext.FiscalAccounts.AddAsync(account, token);
    }

    public async Task Update(FiscalAccount account, CancellationToken token)
    {
        persistenceDbContext.FiscalAccounts.Update(account);
    }

    public async Task<FiscalAccount?> FindById(Guid id, CancellationToken token)
    {
        return await persistenceDbContext.FiscalAccounts
            .Include(a => a.Folios)
            .ThenInclude(f => f.Items)
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken: token);
    }

    public async Task<FiscalAccount> GetById(Guid id, CancellationToken token)
    {
        var result = await FindById(id, token);

        if (result == null)
        {
            throw new NotFoundException($"FiscalAccount with id {id} doesn't exist");
        }

        return result;
    }

    public async Task<IReadOnlyCollection<FiscalAccount>> GetAll(CancellationToken token, Expression<Func<FiscalAccount, bool>>? filter = null)
    {
        IQueryable<FiscalAccount> query = persistenceDbContext.FiscalAccounts
            .Include(a => a.Folios)
            .ThenInclude(f => f.Items);

        if (filter != null)
        {
            query = query.Where(filter);
        }

        return await query.ToListAsync(cancellationToken: token);
    }
}