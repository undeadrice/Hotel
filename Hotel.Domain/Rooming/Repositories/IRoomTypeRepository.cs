using Hotel.Domain.Rooming.Entities;

namespace Hotel.Domain.Rooming.Repositories;

public interface IRoomTypeRepository
{
    Task Add(RoomType roomType, CancellationToken token = default);

    Task<RoomType> GetById(Guid id, CancellationToken token = default);

    Task<RoomType?> FindById(Guid id, CancellationToken token = default);

    Task<bool> ExistsByName(string name, CancellationToken token = default);

    Task<bool> ExistsByNameExcluding(Guid roomTypeId, string name, CancellationToken token = default);
}
