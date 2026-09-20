using Hotel.Application.RatePlans.TransferObjects;
using MediatR;
using Hotel.Application.RatePlans.Repositories;
using Hotel.Application.Pipeline;
using Hotel.Application.Users.Enums;

namespace Hotel.Application.RatePlans.Queries;

[CheckPermission(Permission.RatePlanView)]
public record GetRatePlansQuery : IRequest<IReadOnlyCollection<RatePlanListDto>>;

internal class GetRatePlansQueryHandler(IRatePlanReadRepository ratePlanReadRepository)
    : IRequestHandler<GetRatePlansQuery, IReadOnlyCollection<RatePlanListDto>>
{
    public async Task<IReadOnlyCollection<RatePlanListDto>> Handle(GetRatePlansQuery request, CancellationToken cancellationToken)
    {
        return await ratePlanReadRepository.GetAll(cancellationToken);
    }
}
