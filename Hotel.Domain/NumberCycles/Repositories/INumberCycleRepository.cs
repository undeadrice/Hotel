using Hotel.Domain.NumberCycles.Entities;
using Hotel.Domain.NumberCycles.Enums;

namespace Hotel.Domain.NumberCycles.Repositories;

public interface INumberCycleRepository
{
    Task Add(NumberCycle cycle, CancellationToken token = default);

    Task Delete(NumberCycle cycle, CancellationToken token = default);

    Task<NumberCycle?> FindByTopic(NumberCycleTopic topic, CancellationToken token = default);

    Task<NumberCycle?> FindById(Guid id, CancellationToken token = default);

    Task<NumberCycle> GetByTopic(NumberCycleTopic topic, CancellationToken token = default);

    Task<NumberCycle> GetById(Guid id, CancellationToken token = default);

    Task<bool> ExistsByTopic(NumberCycleTopic topic, CancellationToken token = default);

    Task<IReadOnlyCollection<NumberCycle>> GetAll(CancellationToken token = default);

    Task<long> CountChildren(NumberCycleTopic topic, CancellationToken token = default);
}