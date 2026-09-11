using Hotel.Domain.NumberCycles.Entities;
using Hotel.Domain.NumberCycles.Enums;
using Hotel.Domain.NumberCycles.Exceptions;
using Hotel.Domain.NumberCycles.Repositories;

namespace Hotel.Domain.NumberCycles.Services;

public class NumberCycleService(INumberCycleRepository numberCycleRepository) : INumberCycleService
{
    public async Task<NumberCycle> Create(
        NumberCycleTopic topic,
        string prefix,
        int startIndex,
        CancellationToken token = default)
    {
        if (await numberCycleRepository.ExistsByTopic(topic, token))
        {
            throw new NumberCycleAlreadyExistsException(topic);
        }

        var cycle = NumberCycle.Create(topic, prefix, startIndex);

        await numberCycleRepository.Add(cycle, token);

        return cycle;
    }

    public async Task Delete(Guid id, CancellationToken token = default)
    {
        var cycle = await numberCycleRepository.GetById(id, token);

        var childrenCount = await numberCycleRepository.CountChildren(cycle.Topic, token);

        if (childrenCount > 0)
        {
            throw new NumberCycleHasChildrenException(cycle.Topic);
        }

        await numberCycleRepository.Delete(cycle, token);
    }

    public async Task<string> NextIdentifier(NumberCycleTopic topic, CancellationToken token = default)
    {
        var cycle = await numberCycleRepository.GetByTopic(topic, token);

        return cycle.NextIdentifier();
    }
}