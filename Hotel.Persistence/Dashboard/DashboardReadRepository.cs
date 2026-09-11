using Hotel.Application.Dashboard.TransferObjects;
using Microsoft.EntityFrameworkCore;
using Hotel.Application.Dashboard.Repositories;

namespace Hotel.Persistence.Dashboard;

public class DashboardReadRepository(PersistenceDbContext dbContext) : IDashboardReadRepository
{
    public async Task<DashboardDto> GetDashboard(DateOnly businessDate, CancellationToken cancellationToken)
    {
        var roomCount = await dbContext.Rooms.CountAsync(cancellationToken);

        var occupiedRoomCount = await dbContext.Reservations
            .Where(r => r.StartDate <= businessDate && r.EndDate >= businessDate)
            .Select(r => r.RoomId)
            .Distinct()
            .CountAsync(cancellationToken);

        var guestCount = await dbContext.Guests.CountAsync(cancellationToken);

        var guestsOnSiteCount = await dbContext.Reservations
            .Where(r => r.StartDate <= businessDate
                        && r.EndDate >= businessDate)
            .SelectMany(r => r.Guests)
            .Select(rg => rg.GuestId)
            .Distinct()
            .CountAsync(cancellationToken);

        var occupancyPercentage = roomCount > 0
            ? Math.Round((double)occupiedRoomCount / roomCount * 100, 2)
            : 0;

        return new DashboardDto(
            roomCount,
            occupiedRoomCount,
            guestCount,
            guestsOnSiteCount,
            occupancyPercentage,
            businessDate);
    }
}