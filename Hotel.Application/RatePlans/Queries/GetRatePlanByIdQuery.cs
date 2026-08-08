using Hotel.Application.RatePlans.TransferObjects;
using MediatR;

namespace Hotel.Application.RatePlans.Queries;

public record GetRatePlanByIdQuery(Guid Id) : IRequest<RatePlanDto>;