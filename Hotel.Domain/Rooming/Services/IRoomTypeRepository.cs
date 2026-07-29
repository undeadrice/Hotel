using Hotel.Domain.Rooming.Entities;
using System.Linq.Expressions;

namespace Hotel.Domain.Rooming.Services;

public interface IRoomTypeRepository
{
    Task Add(RoomType roomType, CancellationToken token = default);

    Task Update(RoomType roomType, CancellationToken token = default);

    Task<RoomType> GetById(Guid id, CancellationToken token = default);

    Task<RoomType?> FindById(Guid id, CancellationToken token = default);

    Task<IReadOnlyCollection<RoomType>> GetAll(CancellationToken token, Expression<Func<RoomType, bool>>? filter = null);
}