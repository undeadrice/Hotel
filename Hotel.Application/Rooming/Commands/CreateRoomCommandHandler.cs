using Hotel.Domain.Rooming.Services;
using MediatR;

namespace Hotel.Application.Rooming.Commands;

public class CreateRoomCommandHandler(IRoomCreationService roomCreationService)
    : IRequestHandler<CreateRoomCommand, Guid>
{
    public async Task<Guid> Handle(CreateRoomCommand request, CancellationToken cancellationToken)
    {
        var room = await roomCreationService.CreateRoom(request.RoomNumber, request.RoomTypeId, cancellationToken);

        return room.Id;
    }
}