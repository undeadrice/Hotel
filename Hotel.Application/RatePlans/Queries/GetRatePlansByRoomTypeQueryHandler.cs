using Hotel.Application.RatePlans.TransferObjects;
using MediatR;
using Hotel.Application.RatePlans.Repositories;
using Hotel.Application.Rooming.Repositories;
using Hotel.Application.Pipeline;
using Hotel.Application.Users.Enums;

namespace Hotel.Application.RatePlans.Queries;

[CheckPermission(Permission.RatePlanView)]
public record GetRatePlansByRoomTypeQuery(Guid RoomId, DateOnly StartDate, DateOnly EndDate)
    : IRequest<IReadOnlyCollection<RatePlanListSimpleDto>>;

internal class GetRatePlansByRoomTypeQueryHandler(
    IRoomReadRepository roomReadRepository,
    IRatePlanReadRepository ratePlanReadRepository)
    : IRequestHandler<GetRatePlansByRoomTypeQuery, IReadOnlyCollection<RatePlanListSimpleDto>>
{
    public async Task<IReadOnlyCollection<RatePlanListSimpleDto>> Handle(
        GetRatePlansByRoomTypeQuery request,
        CancellationToken cancellationToken)
    {
        var room = await roomReadRepository.GetById(request.RoomId, cancellationToken);

        return await ratePlanReadRepository.GetByRoomTypeId(
            room.RoomTypeId,
            request.StartDate,
            request.EndDate,
            cancellationToken);
    }
}
