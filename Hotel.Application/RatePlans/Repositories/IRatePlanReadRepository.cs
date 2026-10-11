using Hotel.Application.RatePlans.TransferObjects;

namespace Hotel.Application.RatePlans.Repositories;

public interface IRatePlanReadRepository
{
    Task<IReadOnlyCollection<RatePlanListDto>> GetAll(CancellationToken cancellationToken);

    Task<RatePlanDto> GetById(Guid id, CancellationToken cancellationToken);

    Task<IReadOnlyCollection<RatePlanListSimpleDto>> GetByRoomTypeId(
        Guid roomTypeId,
        DateOnly startDate,
        DateOnly endDate,
        CancellationToken cancellationToken);
}
