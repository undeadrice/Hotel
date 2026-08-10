using Hotel.Application.RatePlans.Services;
using Hotel.Application.RatePlans.TransferObjects;
using Hotel.Application.Rooming.Services;
using Hotel.Shared.Exceptions;
using MediatR;

namespace Hotel.Application.RatePlans.Queries;

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