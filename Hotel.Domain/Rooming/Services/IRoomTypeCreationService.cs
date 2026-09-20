using Hotel.Domain.Rooming.Entities;

namespace Hotel.Domain.Rooming.Services;

public interface IRoomTypeCreationService
{
    Task<RoomType> CreateRoomType(string name, string? description, CancellationToken cancellationToken = default);
}