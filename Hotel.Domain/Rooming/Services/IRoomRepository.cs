using Hotel.Domain.Rooming.Entities;

namespace Hotel.Domain.Rooming.Services;

public interface IRoomRepository
{
    Task Add(Room room, CancellationToken token = default);

    Task Update(Room room, CancellationToken token = default);

    Task<Room> GetById(Guid id, CancellationToken token = default);

    Task<Room?> FindById(Guid id, CancellationToken token = default);

    Task<bool> ExistsByRoomNumber(string roomNumber, CancellationToken token = default);

    Task<bool> ExistsByRoomNumberExcluding(Guid roomId, string roomNumber, CancellationToken token = default);
}
