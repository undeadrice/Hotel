using Hotel.Application.Pipeline;
using Hotel.Application.RatePlans.TransferObjects;
using Hotel.Application.Users.Enums;
using MediatR;

namespace Hotel.Application.RatePlans.Queries;

[CheckPermission(Permission.RatePlanView)]
public record GetRatePlanByIdQuery(Guid Id) : IRequest<RatePlanDto>;
