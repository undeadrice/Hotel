using Hotel.Application.Rooming.Services;
using Hotel.Application.Rooming.TransferObjects;
using MediatR;

namespace Hotel.Application.Rooming.Queries;

internal class GetRoomByIdQueryHandler(IRoomReadRepository roomReadRepository)
    : IRequestHandler<GetRoomByIdQuery, RoomDto>
{
    public async Task<RoomDto> Handle(GetRoomByIdQuery request, CancellationToken cancellationToken)
    {
        return await roomReadRepository.GetById(request.Id, cancellationToken);
    }
}