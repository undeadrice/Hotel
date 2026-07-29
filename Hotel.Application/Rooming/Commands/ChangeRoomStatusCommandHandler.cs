using Hotel.Domain.Rooming.Services;
using MediatR;

namespace Hotel.Application.Rooming.Commands;

public class ChangeRoomStatusCommandHandler(IRoomRepository roomRepository)
    : IRequestHandler<ChangeRoomStatusCommand>
{
    public async Task Handle(ChangeRoomStatusCommand request, CancellationToken cancellationToken)
    {
        var room = await roomRepository.GetById(request.RoomId);
        room.ChangeStatus(request.NewStatus);

        await roomRepository.Update(room);
    }
}
