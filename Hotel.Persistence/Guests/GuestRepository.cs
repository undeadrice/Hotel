using Hotel.Domain.Guests;
using Hotel.Domain.Guests.Services;
using Hotel.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Hotel.Persistence.Guests;

public class GuestRepository(PersistenceDbContext persistenceDbContext) : IGuestRepository
{
    public async Task Add(Guest guest, CancellationToken token)
    {
        await persistenceDbContext.Guests.AddAsync(guest, token);
    }

    public async Task Update(Guest guest, CancellationToken token)
    {
        persistenceDbContext.Guests.Update(guest);
    }

    public async Task<Guest> GetById(Guid id, CancellationToken token)
    {
        var result = await persistenceDbContext.Guests.FirstOrDefaultAsync(x => x.Id == id, cancellationToken: token);

        if (result == null)
        {
            throw new NotFoundException($"Guest with id {id} doesn't exist");
        }

        return result;
    }

    public async Task<Guest?> FindById(Guid id, CancellationToken token)
    {
        return await persistenceDbContext.Guests.FirstOrDefaultAsync(x => x.Id == id, cancellationToken: token);
    }

    public async Task<IReadOnlyCollection<Guest>> GetAll(CancellationToken token, Expression<Func<Guest, bool>>? filter = null)
    {
        if (filter == null)
        {
            return await persistenceDbContext.Guests.ToListAsync(cancellationToken: token);
        }

        return await persistenceDbContext.Guests.Where(filter).ToListAsync(cancellationToken: token);
    }

    public async Task<IReadOnlyCollection<Guest>> Search(
        string? name,
        string? phone,
        string? email,
        string? documentNumber,
        CancellationToken token = default)
    {
        var query = persistenceDbContext.Guests.AsQueryable();

        if (!string.IsNullOrWhiteSpace(name))
        {
            var normalizedName = name.Trim().ToLower();
            query = query.Where(c =>
                (c.FirstName + " " + c.LastName).ToLower().Contains(normalizedName) ||
                c.FirstName.ToLower().Contains(normalizedName) ||
                c.LastName.ToLower().Contains(normalizedName));
        }

        if (!string.IsNullOrWhiteSpace(phone))
        {
            var normalizedPhone = phone.Trim();
            query = query.Where(c => c.Phone.Contains(normalizedPhone));
        }

        if (!string.IsNullOrWhiteSpace(email))
        {
            var normalizedEmail = email.Trim().ToLower();
            query = query.Where(c => c.Email.ToLower().Contains(normalizedEmail));
        }

        if (!string.IsNullOrWhiteSpace(documentNumber))
        {
            var normalizedDoc = documentNumber.Trim();
            query = query.Where(c => c.DocumentNumber.Contains(normalizedDoc));
        }

        return await query.AsNoTracking().ToListAsync(cancellationToken: token);
    }
}