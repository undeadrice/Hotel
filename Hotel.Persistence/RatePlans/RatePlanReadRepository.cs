using Hotel.Application.RatePlans.TransferObjects;
using Hotel.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;
using Hotel.Application.RatePlans.Repositories;

namespace Hotel.Persistence.RatePlans;

public class RatePlanReadRepository(PersistenceDbContext dbContext) : IRatePlanReadRepository
{
    public async Task<IReadOnlyCollection<RatePlanListDto>> GetAll(CancellationToken cancellationToken)
    {
        return await dbContext.RatePlans
            .AsNoTracking()
            .OrderBy(rp => rp.Name)
            .Select(rp => new RatePlanListDto(
                rp.Id,
                rp.Name,
                rp.TransactionCodeId,
                rp.StartDate,
                rp.EndDate,
                rp.IsActive))
            .ToListAsync(cancellationToken);
    }

    public async Task<RatePlanDto> GetById(Guid id, CancellationToken cancellationToken)
    {
        var ratePlan = await dbContext.RatePlans
            .AsNoTracking()
            .Include(rp => rp.Rooms)
            .Where(rp => rp.Id == id)
            .Select(rp => new RatePlanDto(
                rp.Id,
                rp.Name,
                dbContext.TransactionCodes
                    .Where(tc => tc.Id == rp.TransactionCodeId)
                    .Select(tc => tc.Name)
                    .FirstOrDefault() ?? "Unknown",
                rp.StartDate,
                rp.EndDate,
                rp.Rooms
                    .Select(r => new RatePlanRoomDto(
                        dbContext.RoomTypes
                            .Where(rt => rt.Id == r.RoomTypeId)
                            .Select(rt => rt.Name)
                            .FirstOrDefault() ?? "Unknown",
                        r.Price))
                    .ToList()))
            .FirstOrDefaultAsync(cancellationToken);

        if (ratePlan is null)
        {
            throw new NotFoundException($"RatePlan with id {id} doesn't exist");
        }

        return ratePlan;
    }

    public async Task<IReadOnlyCollection<RatePlanListSimpleDto>> GetByRoomTypeId(
        Guid roomTypeId,
        DateOnly startDate,
        DateOnly endDate,
        CancellationToken cancellationToken)
    {
        return await dbContext.RatePlans
            .AsNoTracking()
            .Where(rp =>
                rp.Rooms.Any(r => r.RoomTypeId == roomTypeId) &&
                rp.StartDate <= startDate && rp.EndDate >= endDate)
            .OrderBy(rp => rp.Name)
            .Select(rp => new RatePlanListSimpleDto(rp.Id, rp.Name))
            .ToListAsync(cancellationToken);
    }
}
