using Hotel.Domain.Rooming.Entities;

namespace Hotel.Domain.Rooming.Services;

public interface IRoomCreationService
{
    Task<Room> CreateRoom(string roomNumber, Guid roomTypeId, CancellationToken cancellationToken = default);
}