using Hotel.Application.Rooming.Services;
using Hotel.Application.Rooming.TransferObjects;
using MediatR;

namespace Hotel.Application.Rooming.Queries;

internal class GetRoomsQueryHandler(IRoomReadRepository roomReadRepository)
    : IRequestHandler<GetRoomsQuery, IReadOnlyCollection<RoomListDto>>
{
    public async Task<IReadOnlyCollection<RoomListDto>> Handle(GetRoomsQuery request, CancellationToken cancellationToken)
    {
        return await roomReadRepository.GetAll(cancellationToken);
    }
}