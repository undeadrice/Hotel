using Hotel.Application.Rooming.TransferObjects;

namespace Hotel.Application.Rooming.Services;

public interface IRoomReadRepository
{
    Task<IReadOnlyCollection<RoomListDto>> GetAll(CancellationToken cancellationToken);

    Task<RoomDto> GetById(Guid id, CancellationToken cancellationToken);

    Task<IReadOnlyCollection<RoomListDto>> GetAvailable(DateOnly startDate, DateOnly endDate, CancellationToken cancellationToken);
}
