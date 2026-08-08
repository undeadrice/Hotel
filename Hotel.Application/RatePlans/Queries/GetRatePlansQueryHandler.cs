using Hotel.Application.RatePlans.Services;
using Hotel.Application.RatePlans.TransferObjects;
using MediatR;

namespace Hotel.Application.RatePlans.Queries;

internal class GetRatePlansQueryHandler(IRatePlanReadRepository ratePlanReadRepository)
    : IRequestHandler<GetRatePlansQuery, IReadOnlyCollection<RatePlanListDto>>
{
    public async Task<IReadOnlyCollection<RatePlanListDto>> Handle(GetRatePlansQuery request, CancellationToken cancellationToken)
    {
        return await ratePlanReadRepository.GetAll(cancellationToken);
    }
}