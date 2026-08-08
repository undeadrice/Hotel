using Hotel.Domain.Folios.Entities;
using Hotel.Domain.Folios.Services;
using Hotel.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Hotel.Persistence.Folios;

public class FolioRepository(PersistenceDbContext persistenceDbContext) : IFolioRepository
{
    public async Task Add(Folio folio, CancellationToken token)
    {
        await persistenceDbContext.Folios.AddAsync(folio, token);
    }

    public async Task Update(Folio folio, CancellationToken token)
    {
        persistenceDbContext.Folios.Update(folio);
    }

    public async Task<Folio?> FindById(Guid id, CancellationToken token)
    {
        return await persistenceDbContext.Folios
            .Include(f => f.Items)
            .FirstOrDefaultAsync(f => f.Id == id, cancellationToken: token);
    }

    public async Task<Folio> GetById(Guid id, CancellationToken token)
    {
        var result = await FindById(id, token);

        if (result == null)
        {
            throw new NotFoundException($"Folio with id {id} doesn't exist");
        }

        return result;
    }

    public async Task<IReadOnlyCollection<Folio>> GetAll(CancellationToken token, Expression<Func<Folio, bool>>? filter = null)
    {
        IQueryable<Folio> query = persistenceDbContext.Folios.Include(f => f.Items);

        if (filter != null)
        {
            query = query.Where(filter);
        }

        return await query.ToListAsync(cancellationToken: token);
    }
}