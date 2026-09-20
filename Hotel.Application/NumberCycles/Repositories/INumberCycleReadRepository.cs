using Hotel.Application.NumberCycles.TransferObjects;

namespace Hotel.Application.NumberCycles.Repositories;

public interface INumberCycleReadRepository
{
    Task<IReadOnlyCollection<NumberCycleDto>> GetAll(CancellationToken cancellationToken);

    Task<NumberCycleDto> GetById(Guid id, CancellationToken cancellationToken);
}