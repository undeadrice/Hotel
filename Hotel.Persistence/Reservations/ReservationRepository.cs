using Hotel.Domain.Reservations.Entities;
using Hotel.Domain.Reservations.Services;
using Hotel.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

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

    public async Task<IReadOnlyCollection<Reservation>> GetAll(CancellationToken token, Expression<Func<Reservation, bool>>? filter = null)
    {
        IQueryable<Reservation> query = persistenceDbContext.Reservations.Include(r => r.Guests);

        if (filter != null)
        {
            query = query.Where(filter);
        }

        return await query.ToListAsync(cancellationToken: token);
    }
}