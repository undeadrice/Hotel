using Hotel.Domain.Rooming.Exceptions;

namespace Hotel.Domain.Rooming.Services;

public class RoomUpdateService(IRoomRepository roomRepository) : IRoomUpdateService
{
    public async Task UpdateRoom(Guid roomId, string roomNumber, Guid roomTypeId, CancellationToken cancellationToken = default)
    {
        var room = await roomRepository.GetById(roomId, cancellationToken);

        if (await roomRepository.ExistsByRoomNumberExcluding(roomId, roomNumber, cancellationToken))
        {
            throw new RoomNumberAlreadyExistsException(roomNumber);
        }

        room.UpdateRoomNumber(roomNumber);
        room.ChangeRoomType(roomTypeId);

        await roomRepository.Update(room, cancellationToken);
    }
}