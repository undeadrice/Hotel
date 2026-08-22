using Hotel.Application.Rooming.TransferObjects;
using MediatR;
using Hotel.Application.Rooming.Repositories;

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