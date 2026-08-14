using Hotel.Domain.NumberCycles.Entities;
using Hotel.Domain.NumberCycles.Enums;
using Hotel.Domain.NumberCycles.Services;
using Hotel.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Persistence.NumberCycles;

public class NumberCycleRepository(PersistenceDbContext dbContext) : INumberCycleRepository
{
    public async Task Add(NumberCycle cycle, CancellationToken token = default)
    {
        await dbContext.NumberCycles.AddAsync(cycle, token);
    }

    public Task Delete(NumberCycle cycle, CancellationToken token = default)
    {
        dbContext.NumberCycles.Remove(cycle);
        return Task.CompletedTask;
    }

    public async Task<NumberCycle?> FindByTopic(NumberCycleTopic topic, CancellationToken token = default)
    {
        return await dbContext.NumberCycles.FirstOrDefaultAsync(c => c.Topic == topic, token);
    }

    public async Task<NumberCycle?> FindById(Guid id, CancellationToken token = default)
    {
        return await dbContext.NumberCycles.FirstOrDefaultAsync(c => c.Id == id, token);
    }

    public async Task<NumberCycle> GetByTopic(NumberCycleTopic topic, CancellationToken token = default)
    {
        var result = await FindByTopic(topic, token);

        if (result is null)
        {
            throw new NotFoundException($"Number cycle with topic {topic} doesn't exist");
        }

        return result;
    }

    public async Task<NumberCycle> GetById(Guid id, CancellationToken token = default)
    {
        var result = await FindById(id, token);

        if (result is null)
        {
            throw new NotFoundException($"Number cycle with id {id} doesn't exist");
        }

        return result;
    }

    public async Task<bool> ExistsByTopic(NumberCycleTopic topic, CancellationToken token = default)
    {
        return await dbContext.NumberCycles.AnyAsync(c => c.Topic == topic, token);
    }

    public async Task<IReadOnlyCollection<NumberCycle>> GetAll(CancellationToken token = default)
    {
        return await dbContext.NumberCycles
            .AsNoTracking()
            .OrderBy(c => c.Topic)
            .ToListAsync(token);
    }

    public async Task<long> CountChildren(NumberCycleTopic topic, CancellationToken token = default)
    {
        return topic switch
        {
            NumberCycleTopic.Reservation => await dbContext.Reservations.LongCountAsync(token),
            NumberCycleTopic.FiscalAccount => await dbContext.FiscalAccounts.LongCountAsync(token),
            _ => 0
        };
    }
}