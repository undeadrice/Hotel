using Hotel.Application.RatePlans.TransferObjects;
using MediatR;
using Hotel.Application.RatePlans.Repositories;
using Hotel.Application.Pipeline;
using Hotel.Application.Users.Enums;

namespace Hotel.Application.RatePlans.Queries;

[CheckPermission(Permission.RatePlanView)]
public record GetRatePlanByIdQuery(Guid Id) : IRequest<RatePlanDto>;

internal class GetRatePlanByIdQueryHandler(IRatePlanReadRepository ratePlanReadRepository)
    : IRequestHandler<GetRatePlanByIdQuery, RatePlanDto>
{
    public async Task<RatePlanDto> Handle(GetRatePlanByIdQuery request, CancellationToken cancellationToken)
    {
        return await ratePlanReadRepository.GetById(request.Id, cancellationToken);
    }
}
