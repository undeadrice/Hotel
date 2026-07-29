using Hotel.Application.Rooming.TransferObjects;

namespace Hotel.Application.Rooming.Services;

public interface IRoomTypeReadRepository
{
    Task<IReadOnlyCollection<RoomTypeListDto>> GetAll(CancellationToken cancellationToken);

    Task<RoomTypeDto> GetById(Guid id, CancellationToken cancellationToken);
}
