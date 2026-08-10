using Hotel.Application.RatePlans.TransferObjects;
using MediatR;

namespace Hotel.Application.RatePlans.Queries;

public record GetRatePlansByRoomTypeQuery(Guid RoomId)
    : IRequest<IReadOnlyCollection<RatePlanListSimpleDto>>;
