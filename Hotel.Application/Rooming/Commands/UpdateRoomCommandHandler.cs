using Hotel.Domain.Rooming.Services;
using MediatR;

namespace Hotel.Application.Rooming.Commands;

public class UpdateRoomCommandHandler(IRoomRepository roomRepository)
    : IRequestHandler<UpdateRoomCommand>
{
    public async Task Handle(UpdateRoomCommand request, CancellationToken cancellationToken)
    {
        var room = await roomRepository.GetById(request.Id);
        room.UpdateRoomNumber(request.RoomNumber);
        room.ChangeRoomType(request.RoomTypeId);

        await roomRepository.Update(room);
    }
}