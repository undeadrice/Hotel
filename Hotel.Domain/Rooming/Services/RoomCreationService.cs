using Hotel.Domain.Rooming.Entities;
using Hotel.Domain.Rooming.Exceptions;
using Hotel.Domain.Rooming.Repositories;

namespace Hotel.Domain.Rooming.Services;

public class RoomCreationService(IRoomRepository roomRepository) : IRoomCreationService
{
    public async Task<Room> CreateRoom(string roomNumber, Guid roomTypeId, CancellationToken cancellationToken = default)
    {
        if (await roomRepository.ExistsByRoomNumber(roomNumber, cancellationToken))
        {
            throw new RoomNumberAlreadyExistsException(roomNumber);
        }

        var room = Room.Create(roomNumber, roomTypeId);

        await roomRepository.Add(room, cancellationToken);

        return room;
    }
}