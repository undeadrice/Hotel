using Hotel.Application.Rooming.Services;
using Hotel.Application.Rooming.TransferObjects;
using Hotel.Domain.Reservations.Enums;
using Hotel.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Persistence.Rooming;

public class RoomReadRepository(PersistenceDbContext dbContext) : IRoomReadRepository
{
    public async Task<IReadOnlyCollection<RoomListDto>> GetAll(CancellationToken cancellationToken)
    {
        return await dbContext.Rooms
            .AsNoTracking()
            .OrderBy(r => r.RoomNumber)
            .Select(r => new RoomListDto(r.Id, r.RoomNumber))
            .ToListAsync(cancellationToken);
    }

    public async Task<RoomDto> GetById(Guid id, CancellationToken cancellationToken)
    {
        var room = await (
            from r in dbContext.Rooms.AsNoTracking()
            join rt in dbContext.RoomTypes.AsNoTracking() on r.RoomTypeId equals rt.Id
            where r.Id == id
            select new RoomDto(r.Id, r.RoomNumber, r.RoomTypeId, rt.Name, r.IsActive)
        ).FirstOrDefaultAsync(cancellationToken);

        if (room is null)
        {
            throw new NotFoundException($"Room with id {id} doesn't exist");
        }

        return room;
    }

    public async Task<IReadOnlyCollection<RoomListDto>> GetAvailable(
        DateOnly startDate,
        DateOnly endDate,
        CancellationToken cancellationToken)
    {
        var reservedRoomIds = dbContext.Reservations
            .AsNoTracking()
            .Where(r => r.Status != ReservationStatus.NoShow && r.StartDate <= endDate && r.EndDate >= startDate)
            .Select(r => r.RoomId)
            .Distinct();

        var roomTypeIdsWithRatePlans = dbContext.RatePlans
            .AsNoTracking()
            .Where(rp => rp.StartDate <= startDate && rp.EndDate >= endDate)
            .SelectMany(rp => rp.Rooms.Select(rpr => rpr.RoomTypeId))
            .Distinct();

        return await dbContext.Rooms
            .AsNoTracking()
            .Where(r =>
                !reservedRoomIds.Contains(r.Id) &&
                r.IsActive &&
                roomTypeIdsWithRatePlans.Contains(r.RoomTypeId))
            .OrderBy(r => r.RoomNumber)
            .Select(r => new RoomListDto(r.Id, r.RoomNumber))
            .ToListAsync(cancellationToken);
    }
}
