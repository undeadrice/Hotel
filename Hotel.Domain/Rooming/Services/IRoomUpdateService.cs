namespace Hotel.Domain.Rooming.Services;

public interface IRoomUpdateService
{
    Task UpdateRoom(Guid roomId, string roomNumber, Guid roomTypeId, CancellationToken cancellationToken = default);
}