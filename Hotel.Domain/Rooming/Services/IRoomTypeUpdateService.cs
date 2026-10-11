namespace Hotel.Domain.Rooming.Services;

public interface IRoomTypeUpdateService
{
    Task UpdateRoomType(Guid roomTypeId, string name, string? description, CancellationToken cancellationToken = default);
}