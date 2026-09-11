using Hotel.Domain.Rooming.Exceptions;
using Hotel.Domain.Rooming.Repositories;

namespace Hotel.Domain.Rooming.Services;

public class RoomTypeUpdateService(IRoomTypeRepository roomTypeRepository) : IRoomTypeUpdateService
{
    public async Task UpdateRoomType(Guid roomTypeId, string name, string? description, CancellationToken cancellationToken = default)
    {
        var roomType = await roomTypeRepository.GetById(roomTypeId, cancellationToken);

        if (await roomTypeRepository.ExistsByNameExcluding(roomTypeId, name, cancellationToken))
        {
            throw new RoomTypeNameAlreadyExistsException(name);
        }

        roomType.Update(name, description);
    }
}