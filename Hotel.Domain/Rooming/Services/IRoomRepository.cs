using Hotel.Domain.Rooming.Entities;
using System.Linq.Expressions;

namespace Hotel.Domain.Rooming.Services;

public interface IRoomRepository
{
    Task Add(Room room, CancellationToken token = default);

    Task Update(Room room, CancellationToken token = default);

    Task<Room> GetById(Guid id, CancellationToken token = default);

    Task<Room?> FindById(Guid id, CancellationToken token = default);

    Task<IReadOnlyCollection<Room>> GetAll(CancellationToken token, Expression<Func<Room, bool>>? filter = null);

    Task<bool> ExistsByRoomNumber(string roomNumber, CancellationToken token = default);
}