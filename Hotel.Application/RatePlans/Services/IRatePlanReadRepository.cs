using Hotel.Application.RatePlans.TransferObjects;

namespace Hotel.Application.RatePlans.Services;

public interface IRatePlanReadRepository
{
    Task<IReadOnlyCollection<RatePlanListDto>> GetAll(CancellationToken cancellationToken);

    Task<RatePlanDto> GetById(Guid id, CancellationToken cancellationToken);
}