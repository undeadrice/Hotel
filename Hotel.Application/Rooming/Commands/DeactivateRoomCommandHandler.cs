using Hotel.Domain.Rooming.Services;
using MediatR;
using Hotel.Domain.Rooming.Repositories;
using Hotel.Application.Pipeline;
using Hotel.Application.Users.Enums;

namespace Hotel.Application.Rooming.Commands;

[CheckPermission(Permission.RoomDelete)]
public record DeactivateRoomCommand(Guid RoomId) : ICommand;

public class DeactivateRoomCommandHandler(IRoomRepository roomRepository)
    : IRequestHandler<DeactivateRoomCommand>
{
    public async Task Handle(DeactivateRoomCommand request, CancellationToken cancellationToken)
    {
        var room = await roomRepository.GetById(request.RoomId, cancellationToken);

        room.Deactivate();
    }
}
