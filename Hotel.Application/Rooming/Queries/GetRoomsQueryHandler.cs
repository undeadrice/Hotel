using Hotel.Application.Rooming.TransferObjects;
using MediatR;
using Hotel.Application.Rooming.Repositories;

namespace Hotel.Application.Rooming.Queries;

internal class GetRoomsQueryHandler(IRoomReadRepository roomReadRepository)
    : IRequestHandler<GetRoomsQuery, IReadOnlyCollection<RoomListDto>>
{
    public async Task<IReadOnlyCollection<RoomListDto>> Handle(GetRoomsQuery request, CancellationToken cancellationToken)
    {
        return await roomReadRepository.GetAll(cancellationToken);
    }
}