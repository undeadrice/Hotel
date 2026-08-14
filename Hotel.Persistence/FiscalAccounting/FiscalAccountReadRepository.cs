using Hotel.Application.FiscalAccounting.Services;
using Hotel.Application.FiscalAccounting.TransferObjects;
using Hotel.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Persistence.FiscalAccounting;

public class FiscalAccountReadRepository(PersistenceDbContext dbContext) : IFiscalAccountReadRepository
{
    public async Task<IReadOnlyCollection<FiscalAccountListItemDto>> GetAll(CancellationToken cancellationToken)
    {
        return await dbContext.FiscalAccounts
            .AsNoTracking()
            .OrderBy(a => a.CreatedAt)
            .Select(a => new FiscalAccountListItemDto(
                a.Id,
                a.CycleIdentifier,
                a.CreatedAt,
                dbContext.Guests
                    .Where(g => g.Id == a.OwnerId)
                    .Select(g => g.FirstName + " " + g.LastName)
                    .FirstOrDefault() ?? "Unknown"))
            .ToListAsync(cancellationToken);
    }

    public async Task<FiscalAccountDetailsDto> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var account = await dbContext.FiscalAccounts
            .AsNoTracking()
            .Where(a => a.Id == id)
            .Select(a => new FiscalAccountDetailsDto(
                a.Id,
                a.OriginatorId,
                a.CycleIdentifier,
                dbContext.Guests
                    .Where(g => g.Id == a.OwnerId)
                    .Select(g => g.FirstName + " " + g.LastName)
                    .FirstOrDefault() ?? "Unknown",
                a.CreatedAt,
                a.Folios
                    .Select(f => new FolioDto(
                        f.Id,
                        f.CreatedAt,
                        f.Items
                            .Select(i => new FolioItemDto(
                                i.Id,
                                i.Description,
                                i.Amount,
                                i.CreatedAt))
                            .ToList()
                            .AsReadOnly()))
                    .ToList()
                    .AsReadOnly()))
            .FirstOrDefaultAsync(cancellationToken);

        if (account is null)
        {
            throw new NotFoundException($"FiscalAccount with id {id} doesn't exist");
        }

        return account;
    }
}