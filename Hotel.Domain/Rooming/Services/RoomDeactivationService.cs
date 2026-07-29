using Hotel.Domain.Rooming.Entities;
using Hotel.Domain.Rooming.Exceptions;

namespace Hotel.Domain.Rooming.Services;

public class RoomDeactivationService(IRoomRepository roomRepository) : IRoomDeactivationService
{
    public async Task DeactivateRoom(Guid roomId, CancellationToken cancellationToken = default)
    {
        var room = await roomRepository.GetById(roomId, cancellationToken);

        if (!room.IsActive)
        {
            throw new RoomStatusChangeInvalidException("Room is already deactivated.");
        }

        room.Deactivate();

        await roomRepository.Update(room, cancellationToken);
    }
}