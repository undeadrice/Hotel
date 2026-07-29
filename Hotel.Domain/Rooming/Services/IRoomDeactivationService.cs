namespace Hotel.Domain.Rooming.Services;

public interface IRoomDeactivationService
{
    Task DeactivateRoom(Guid roomId, CancellationToken cancellationToken = default);
}