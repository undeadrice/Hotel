using Hotel.Domain.RatePlans.Entities;
using Hotel.Domain.RatePlans.Services;
using Hotel.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Persistence.RatePlans;

public class RatePlanRepository(PersistenceDbContext persistenceDbContext) : IRatePlanRepository
{
    public async Task Add(RatePlan ratePlan, CancellationToken token)
    {
        await persistenceDbContext.RatePlans.AddAsync(ratePlan, token);
    }

    public async Task Update(RatePlan ratePlan, CancellationToken token)
    {
        persistenceDbContext.RatePlans.Update(ratePlan);
    }

    public async Task<RatePlan> GetById(Guid id, CancellationToken token)
    {
        var result = await persistenceDbContext.RatePlans
            .Include(rp => rp.Rooms)
            .FirstOrDefaultAsync(x => x.Id == id, token);

        if (result == null)
        {
            throw new NotFoundException($"RatePlan with id {id} doesn't exist");
        }

        return result;
    }

    public async Task<RatePlan?> FindById(Guid id, CancellationToken token)
    {
        return await persistenceDbContext.RatePlans
            .Include(rp => rp.Rooms)
            .FirstOrDefaultAsync(x => x.Id == id, token);
    }
}
