using Hotel.Application.Reservations.Services;
using Hotel.Application.Reservations.TransferObjects;
using Hotel.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;

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
                r.CreatorId,
                r.RoomId,
                r.RatePlanId,
                r.StartDate,
                r.EndDate,
                r.ArrivalTime,
                r.CreatedAt,
                r.Guests.Select(g => g.GuestId).ToList()))
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
                r.StartDate,
                r.EndDate,
                r.ArrivalTime,
                r.CreatedAt,
                r.Guests.Select(g => g.GuestId).ToList()))
            .FirstOrDefaultAsync(cancellationToken);

        if (reservation is null)
        {
            throw new NotFoundException($"Reservation with id {id} doesn't exist");
        }

        return reservation;
    }
}