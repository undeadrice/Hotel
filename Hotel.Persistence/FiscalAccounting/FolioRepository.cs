using Hotel.Domain.FiscalAccounting.Entities;
using Hotel.Domain.FiscalAccounting.Services;
using Hotel.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Persistence.FiscalAccounting;

public class FolioRepository(PersistenceDbContext dbContext) : IFolioRepository
{
    public async Task<Folio> GetById(Guid id, CancellationToken token = default)
    {
        var folio = await dbContext
            .Set<Folio>()
            .Include(f => f.Items)
            .FirstOrDefaultAsync(f => f.Id == id, token);

        if (folio is null)
        {
            throw new NotFoundException($"Folio with id {id} doesn't exist");
        }

        return folio;
    }
}