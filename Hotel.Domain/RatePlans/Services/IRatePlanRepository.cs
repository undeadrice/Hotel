using Hotel.Domain.RatePlans.Entities;
using System.Linq.Expressions;

namespace Hotel.Domain.RatePlans.Services;

public interface IRatePlanRepository
{
    Task Add(RatePlan ratePlan, CancellationToken token = default);

    Task Update(RatePlan ratePlan, CancellationToken token = default);

    Task<RatePlan> GetById(Guid id, CancellationToken token = default);

    Task<RatePlan?> FindById(Guid id, CancellationToken token = default);

    Task<IReadOnlyCollection<RatePlan>> GetAll(CancellationToken token, Expression<Func<RatePlan, bool>>? filter = null);
}