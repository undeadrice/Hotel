using Hotel.Domain.Rooming.Exceptions;

namespace Hotel.Domain.Rooming.Services;

public class RoomUpdateService(IRoomRepository roomRepository) : IRoomUpdateService
{
    public async Task UpdateRoom(Guid roomId, string roomNumber, Guid roomTypeId, CancellationToken cancellationToken = default)
    {
        var room = await roomRepository.GetById(roomId, cancellationToken);

        if (room.RoomNumber != roomNumber && await roomRepository.ExistsByRoomNumber(roomNumber, cancellationToken))
        {
            throw new RoomNumberAlreadyExistsException(roomNumber);
        }

        if(roomNumber != room.RoomNumber)
        {
            room.UpdateRoomNumber(roomNumber);
        }

        if(room.RoomTypeId != roomTypeId)
        {
            room.ChangeRoomType(roomTypeId);
        }
    }
}