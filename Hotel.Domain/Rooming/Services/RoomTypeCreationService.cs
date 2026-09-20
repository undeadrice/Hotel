using Hotel.Domain.Rooming.Entities;
using Hotel.Domain.Rooming.Exceptions;
using Hotel.Domain.Rooming.Repositories;

namespace Hotel.Domain.Rooming.Services;

public class RoomTypeCreationService(IRoomTypeRepository roomTypeRepository) : IRoomTypeCreationService
{
    public async Task<RoomType> CreateRoomType(string name, string? description, CancellationToken cancellationToken = default)
    {
        if (await roomTypeRepository.ExistsByName(name, cancellationToken))
        {
            throw new RoomTypeNameAlreadyExistsException(name);
        }

        var roomType = RoomType.Create(name, description);

        await roomTypeRepository.Add(roomType, cancellationToken);

        return roomType;
    }
}