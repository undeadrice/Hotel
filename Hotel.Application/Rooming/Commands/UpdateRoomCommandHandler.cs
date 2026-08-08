using Hotel.Domain.Rooming.Services;
using MediatR;

namespace Hotel.Application.Rooming.Commands;

public class UpdateRoomCommandHandler(IRoomUpdateService roomUpdateService)
    : IRequestHandler<UpdateRoomCommand>
{
    public async Task Handle(UpdateRoomCommand request, CancellationToken cancellationToken)
    {
        await roomUpdateService.UpdateRoom(request.Id, request.RoomNumber, request.RoomTypeId, cancellationToken);
    }
}