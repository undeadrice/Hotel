using Hotel.Application.Rooming.TransferObjects;
using MediatR;
using Hotel.Application.Rooming.Repositories;

namespace Hotel.Application.Rooming.Queries;

internal class GetRoomTypesQueryHandler(IRoomTypeReadRepository roomTypeReadRepository)
    : IRequestHandler<GetRoomTypesQuery, IReadOnlyCollection<RoomTypeListDto>>
{
    public async Task<IReadOnlyCollection<RoomTypeListDto>> Handle(GetRoomTypesQuery request, CancellationToken cancellationToken)
    {
        return await roomTypeReadRepository.GetAll(cancellationToken);
    }
}