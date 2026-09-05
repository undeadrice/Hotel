using Hotel.Domain.Rooming.Services;
using MediatR;
using Hotel.Application.Pipeline;
using Hotel.Application.Users.Enums;

namespace Hotel.Application.Rooming.Commands;

[CheckPermission(Permission.RoomCreate)]
public record CreateRoomCommand(
    string RoomNumber,
    Guid RoomTypeId)
    : ICommand<Guid>;

public class CreateRoomCommandHandler(IRoomCreationService roomCreationService)
    : IRequestHandler<CreateRoomCommand, Guid>
{
    public async Task<Guid> Handle(CreateRoomCommand request, CancellationToken cancellationToken)
    {
        var room = await roomCreationService.CreateRoom(request.RoomNumber, request.RoomTypeId, cancellationToken);

        return room.Id;
    }
}
