using Hotel.Domain.NumberCycles.Entities;
using Hotel.Domain.NumberCycles.Enums;

namespace Hotel.Domain.NumberCycles.Services;

public interface INumberCycleService
{
    Task<NumberCycle> Create(NumberCycleTopic topic, string prefix, int startIndex, CancellationToken token = default);

    Task Delete(Guid id, CancellationToken token = default);

    Task<string> NextIdentifier(NumberCycleTopic topic, CancellationToken token = default);
}