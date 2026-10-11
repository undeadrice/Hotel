namespace Hotel.Domain.Reservations.Services;

public interface IRoomAvailabilityService
{
    Task<bool> IsRoomOccupied(Guid roomId, DateOnly startDate, DateOnly endDate, CancellationToken token = default);
}
