using Hotel.Application.RatePlans.TransferObjects;
using MediatR;

namespace Hotel.Application.RatePlans.Queries;

public record GetRatePlansQuery : IRequest<IReadOnlyCollection<RatePlanListDto>>;