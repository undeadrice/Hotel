using Hotel.Application.Reservations.TransferObjects;
using Hotel.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;
using Hotel.Application.Reservations.Repositories;

namespace Hotel.Persistence.Reservations;

public class ReservationReadRepository(PersistenceDbContext dbContext) : IReservationReadRepository
{
    public async Task<IReadOnlyCollection<ReservationListDto>> GetAll(CancellationToken cancellationToken)
    {
        return await dbContext.Reservations
            .AsNoTracking()
            .Include(r => r.Guests)
            .OrderByDescending(r => r.CreatedAt)
            .Select(r => new ReservationListDto(
                r.Id,
                r.CycleIdentifier,
                dbContext.Rooms
                    .Where(room => room.Id == r.RoomId)
                    .Select(room => room.RoomNumber)
                    .FirstOrDefault() ?? "Unknown",
                dbContext.RatePlans
                    .Where(rp => rp.Id == r.RatePlanId)
                    .Select(rp => rp.Name)
                    .FirstOrDefault() ?? "Unknown",
                dbContext.Guests
                    .Where(g => g.Id == r.CreatorId)
                    .Select(g => g.FirstName + " " + g.LastName)
                    .FirstOrDefault() ?? "Unknown",
                r.StartDate,
                r.EndDate,
                r.ArrivalTime,
                r.CreatedAt,
                r.Status,
                r.Guests.Count))
            .ToListAsync(cancellationToken);
    }

    public async Task<ReservationDto> GetById(Guid id, CancellationToken cancellationToken)
    {
        var reservation = await dbContext.Reservations
            .AsNoTracking()
            .Include(r => r.Guests)
            .Where(r => r.Id == id)
            .Select(r => new ReservationDto(
                r.Id,
                r.CreatorId,
                r.RoomId,
                r.RatePlanId,
                r.CycleIdentifier,
                r.StartDate,
                r.EndDate,
                r.ArrivalTime,
                r.CreatedAt,
                r.Status,
                r.Guests.Select(g => g.GuestId).ToList()))
            .FirstOrDefaultAsync(cancellationToken);

        if (reservation is null)
        {
            throw new NotFoundException($"Reservation with id {id} doesn't exist");
        }

        return reservation;
    }
}