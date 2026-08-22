using Hotel.Domain.RatePlans.Entities;

namespace Hotel.Domain.RatePlans.Repositories;

public interface IRatePlanRepository
{
    Task Add(RatePlan ratePlan, CancellationToken token = default);

    Task<RatePlan> GetById(Guid id, CancellationToken token = default);

    Task<RatePlan?> FindById(Guid id, CancellationToken token = default);
}
