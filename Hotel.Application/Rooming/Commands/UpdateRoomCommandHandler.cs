using Hotel.Domain.Rooming.Services;
using MediatR;
using Hotel.Application.Pipeline;
using Hotel.Application.Users.Enums;

namespace Hotel.Application.Rooming.Commands;

[CheckPermission(Permission.RoomEdit)]
public record UpdateRoomCommand(
    Guid Id,
    string RoomNumber,
    Guid RoomTypeId)
    : ICommand;

public class UpdateRoomCommandHandler(IRoomUpdateService roomUpdateService)
    : IRequestHandler<UpdateRoomCommand>
{
    public async Task Handle(UpdateRoomCommand request, CancellationToken cancellationToken)
    {
        await roomUpdateService.UpdateRoom(request.Id, request.RoomNumber, request.RoomTypeId, cancellationToken);
    }
}
