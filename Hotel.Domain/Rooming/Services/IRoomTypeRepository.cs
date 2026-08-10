using Hotel.Domain.Rooming.Entities;

namespace Hotel.Domain.Rooming.Services;

public interface IRoomTypeRepository
{
    Task Add(RoomType roomType, CancellationToken token = default);

    Task Update(RoomType roomType, CancellationToken token = default);

    Task<RoomType> GetById(Guid id, CancellationToken token = default);

    Task<RoomType?> FindById(Guid id, CancellationToken token = default);
}
