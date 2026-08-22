using Hotel.Domain.Rooming.Services;
using MediatR;

namespace Hotel.Application.Rooming.Commands;

public class DeactivateRoomCommandHandler(IRoomRepository roomRepository)
    : IRequestHandler<DeactivateRoomCommand>
{
    public async Task Handle(DeactivateRoomCommand request, CancellationToken cancellationToken)
    {
        var room = await roomRepository.GetById(request.RoomId, cancellationToken);

        room.Deactivate();
    }
}