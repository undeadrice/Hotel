using Hotel.Domain.Reservations.Repositories;
namespace Hotel.Domain.Reservations.Services;

public class RoomAvailabilityService(IReservationRepository reservationRepository) : IRoomAvailabilityService
{
    public async Task<bool> IsRoomOccupied(Guid roomId, DateOnly startDate, DateOnly endDate, CancellationToken token = default)
    {
        return await reservationRepository.HasOverlappingReservation(roomId, startDate, endDate, token);
    }
}