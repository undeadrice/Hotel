using Hotel.Domain.Persistence;
using Microsoft.EntityFrameworkCore.Storage;

namespace Hotel.Persistence.Services;

public class UnitOfWork(PersistenceDbContext dbContext) : IUnitOfWork
{
    private IDbContextTransaction? _transaction;

    public async Task StartTransaction()
    {
        if (_transaction != null)
        {
            return;
        }

        _transaction = await dbContext.Database.BeginTransactionAsync();
    }

    public async Task CommitAsync()
    {
        await dbContext.SaveChangesAsync();

        if (_transaction != null)
        {
            await _transaction.CommitAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task RollbackAsync()
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }
}
