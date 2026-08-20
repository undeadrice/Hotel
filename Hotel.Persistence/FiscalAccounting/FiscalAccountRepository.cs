using Hotel.Domain.FiscalAccounting.Entities;
using Hotel.Domain.FiscalAccounting.Services;
using Hotel.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Persistence.FiscalAccounting;

public class FiscalAccountRepository(PersistenceDbContext persistenceDbContext) : IFiscalAccountRepository
{
    public async Task Add(FiscalAccount account, CancellationToken token)
    {
        await persistenceDbContext.FiscalAccounts.AddAsync(account, token);
    }

    public Task Update(FiscalAccount account, CancellationToken token)
    {
        persistenceDbContext.FiscalAccounts.Update(account);
        return Task.CompletedTask;
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

    public async Task<FiscalAccount> GetByFolioId(Guid folioId, CancellationToken token)
    {
        var account = await persistenceDbContext.FiscalAccounts
            .Include(a => a.Folios)
            .ThenInclude(f => f.Items)
            .FirstOrDefaultAsync(a => a.Folios.Any(f => f.Id == folioId), cancellationToken: token);

        if (account is null)
        {
            throw new NotFoundException($"FiscalAccount containing folio {folioId} doesn't exist");
        }

        return account;
    }

    public async Task<FiscalAccount> GetByOriginatorId(Guid originatorId, CancellationToken token)
    {
        var account = await persistenceDbContext.FiscalAccounts
            .Include(a => a.Folios)
            .ThenInclude(f => f.Items)
            .FirstOrDefaultAsync(a => a.OriginatorId == originatorId, cancellationToken: token);

        if (account is null)
        {
            throw new NotFoundException($"FiscalAccount with originator id {originatorId} doesn't exist");
        }

        return account;
    }

    public async Task<FiscalAccount> GetForSettlement(Guid accountId, Guid folioId, CancellationToken token)
    {
        var account = await persistenceDbContext.FiscalAccounts
            .Include(a => a.Folios.Where(f => f.Id == folioId))
            .ThenInclude(f => f.Items)
            .FirstOrDefaultAsync(a => a.Id == accountId, cancellationToken: token);

        if (account is null)
        {
            throw new NotFoundException($"FiscalAccount with id {accountId} doesn't exist");
        }

        return account;
    }

    public async Task<FiscalAccount> GetForCheckOut(Guid accountId, CancellationToken token)
    {
        var account = await persistenceDbContext.FiscalAccounts
            .Include(a => a.Folios)
            .FirstOrDefaultAsync(a => a.Id == accountId, cancellationToken: token);

        if (account is null)
        {
            throw new NotFoundException($"FiscalAccount with id {accountId} doesn't exist");
        }

        return account;
    }
}
