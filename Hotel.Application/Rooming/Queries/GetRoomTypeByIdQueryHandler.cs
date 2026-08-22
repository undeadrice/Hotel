using Hotel.Application.Rooming.TransferObjects;
using MediatR;
using Hotel.Application.Rooming.Repositories;

namespace Hotel.Application.Rooming.Queries;

internal class GetRoomTypeByIdQueryHandler(IRoomTypeReadRepository roomTypeReadRepository)
    : IRequestHandler<GetRoomTypeByIdQuery, RoomTypeDto>
{
    public async Task<RoomTypeDto> Handle(GetRoomTypeByIdQuery request, CancellationToken cancellationToken)
    {
        return await roomTypeReadRepository.GetById(request.Id, cancellationToken);
    }
}