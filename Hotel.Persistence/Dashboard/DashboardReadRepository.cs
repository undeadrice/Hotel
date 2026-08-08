using Hotel.Application.Dashboard.Services;
using Hotel.Application.Dashboard.TransferObjects;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Persistence.Dashboard;

public class DashboardReadRepository(PersistenceDbContext dbContext) : IDashboardReadRepository
{
    public async Task<DashboardDto> GetDashboard(CancellationToken cancellationToken)
    {
        var roomCount = await dbContext.Rooms.CountAsync(cancellationToken);
        var guestCount = await dbContext.Guests.CountAsync(cancellationToken);

        return new DashboardDto(roomCount, guestCount);
    }
}