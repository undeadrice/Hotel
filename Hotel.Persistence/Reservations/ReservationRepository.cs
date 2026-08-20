using Hotel.Domain.Reservations.Entities;
using Hotel.Domain.Reservations.Enums;
using Hotel.Domain.Reservations.Services;
using Hotel.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Persistence.Reservations;

public class ReservationRepository(PersistenceDbContext persistenceDbContext) : IReservationRepository
{
    public async Task Add(Reservation reservation, CancellationToken token)
    {
        await persistenceDbContext.Reservations.AddAsync(reservation, token);
    }

    public async Task Update(Reservation reservation, CancellationToken token)
    {
        persistenceDbContext.Reservations.Update(reservation);
    }

    public async Task<Reservation?> FindById(Guid id, CancellationToken token)
    {
        return await persistenceDbContext.Reservations
            .Include(r => r.Guests)
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken: token);
    }

    public async Task<Reservation> GetById(Guid id, CancellationToken token)
    {
        var result = await FindById(id, token);

        if (result == null)
        {
            throw new NotFoundException($"Reservation with id {id} doesn't exist");
        }

        return result;
    }

    public async Task<bool> HasOverlappingReservation(Guid roomId, DateOnly startDate, DateOnly endDate, CancellationToken token)
    {
        return await persistenceDbContext.Reservations
            .AnyAsync(r => r.RoomId == roomId
                         && r.Status != ReservationStatus.NoShow
                         && r.StartDate <= endDate
                         && r.EndDate >= startDate, cancellationToken: token);
    }

    public async Task<IReadOnlyCollection<Reservation>> GetForEndOfDay(DateOnly businessDate, CancellationToken token)
    {
        return await persistenceDbContext.Reservations
            .Where(r => r.Status == ReservationStatus.Reserved && r.StartDate == businessDate
                     || r.Status == ReservationStatus.DueIn)
            .ToListAsync(cancellationToken: token);
    }

    public async Task<IReadOnlyCollection<Reservation>> GetInHouseForBusinessDate(DateOnly businessDate, CancellationToken token)
    {
        return await persistenceDbContext.Reservations
            .Where(r => r.Status == ReservationStatus.InHouse
                     && r.StartDate <= businessDate
                     && r.EndDate > businessDate)
            .ToListAsync(cancellationToken: token);
    }
}
