using Hotel.Application.Rooming.Services;
using Hotel.Application.Rooming.TransferObjects;
using MediatR;

namespace Hotel.Application.Rooming.Queries;

internal class GetRoomTypesQueryHandler(IRoomTypeReadRepository roomTypeReadRepository)
    : IRequestHandler<GetRoomTypesQuery, IReadOnlyCollection<RoomTypeListDto>>
{
    public async Task<IReadOnlyCollection<RoomTypeListDto>> Handle(GetRoomTypesQuery request, CancellationToken cancellationToken)
    {
        return await roomTypeReadRepository.GetAll(cancellationToken);
    }
}