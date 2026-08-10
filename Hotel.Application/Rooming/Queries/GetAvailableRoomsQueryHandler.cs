using Hotel.Application.Rooming.Services;
using Hotel.Application.Rooming.TransferObjects;
using MediatR;

namespace Hotel.Application.Rooming.Queries;

internal class GetAvailableRoomsQueryHandler(IRoomReadRepository roomReadRepository)
    : IRequestHandler<GetAvailableRoomsQuery, IReadOnlyCollection<RoomListDto>>
{
    public async Task<IReadOnlyCollection<RoomListDto>> Handle(
        GetAvailableRoomsQuery request,
        CancellationToken cancellationToken)
    {
        return await roomReadRepository.GetAvailable(
            request.StartDate,
            request.EndDate,
            cancellationToken);
    }
}