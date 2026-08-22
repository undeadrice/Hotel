using Hotel.Application.RatePlans.TransferObjects;
using MediatR;
using Hotel.Application.RatePlans.Repositories;

namespace Hotel.Application.RatePlans.Queries;

internal class GetRatePlanByIdQueryHandler(IRatePlanReadRepository ratePlanReadRepository)
    : IRequestHandler<GetRatePlanByIdQuery, RatePlanDto>
{
    public async Task<RatePlanDto> Handle(GetRatePlanByIdQuery request, CancellationToken cancellationToken)
    {
        return await ratePlanReadRepository.GetById(request.Id, cancellationToken);
    }
}