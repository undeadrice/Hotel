using Hotel.Application.Dashboard.Services;
using Hotel.Application.Dashboard.TransferObjects;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Persistence.Dashboard;

public class DashboardReadRepository(PersistenceDbContext dbContext) : IDashboardReadRepository
{
    public async Task<DashboardDto> GetDashboard(CancellationToken cancellationToken)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        var roomCount = await dbContext.Rooms.CountAsync(cancellationToken);

        var occupiedRoomCount = await dbContext.Reservations
            .Where(r => r.StartDate <= today && r.EndDate >= today)
            .Select(r => r.RoomId)
            .Distinct()
            .CountAsync(cancellationToken);

        var guestCount = await dbContext.Guests.CountAsync(cancellationToken);

        var guestsOnSiteCount = await dbContext.Reservations
            .Where(r => r.StartDate <= today
                        && r.EndDate >= today)
            .SelectMany(r => r.Guests)
            .Select(rg => rg.GuestId)
            .Distinct()
            .CountAsync(cancellationToken);

        var occupancyPercentage = roomCount > 0
            ? Math.Round((double)occupiedRoomCount / roomCount * 100, 2)
            : 0;

        return new DashboardDto(roomCount, occupiedRoomCount, guestCount, guestsOnSiteCount, occupancyPercentage);
    }
}
