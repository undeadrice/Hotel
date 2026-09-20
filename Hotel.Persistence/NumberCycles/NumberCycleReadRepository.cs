using Hotel.Application.NumberCycles.TransferObjects;
using Hotel.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;
using Hotel.Application.NumberCycles.Repositories;

namespace Hotel.Persistence.NumberCycles;

public class NumberCycleReadRepository(PersistenceDbContext dbContext) : INumberCycleReadRepository
{
    public async Task<IReadOnlyCollection<NumberCycleDto>> GetAll(CancellationToken cancellationToken)
    {
        return await dbContext.NumberCycles
            .AsNoTracking()
            .OrderBy(c => c.Topic)
            .Select(c => new NumberCycleDto(
                c.Id,
                c.Topic,
                c.Prefix,
                c.StartIndex,
                c.CurrentIndex,
                c.CreatedAt))
            .ToListAsync(cancellationToken);
    }

    public async Task<NumberCycleDto> GetById(Guid id, CancellationToken cancellationToken)
    {
        var cycle = await dbContext.NumberCycles
            .AsNoTracking()
            .Where(c => c.Id == id)
            .Select(c => new NumberCycleDto(
                c.Id,
                c.Topic,
                c.Prefix,
                c.StartIndex,
                c.CurrentIndex,
                c.CreatedAt))
            .FirstOrDefaultAsync(cancellationToken);

        if (cycle is null)
        {
            throw new NotFoundException($"Number cycle with id {id} doesn't exist");
        }

        return cycle;
    }
}