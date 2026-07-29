using Hotel.Domain.Rooming.Services;
using MediatR;

namespace Hotel.Application.Rooming.Commands;

public class DeactivateRoomCommandHandler(IRoomDeactivationService deactivationService)
    : IRequestHandler<DeactivateRoomCommand>
{
    public async Task Handle(DeactivateRoomCommand request, CancellationToken cancellationToken)
    {
        await deactivationService.DeactivateRoom(request.RoomId, cancellationToken);
    }
}