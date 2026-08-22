using Hotel.Application.Guests.TransferObjects;
using Hotel.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;
using Hotel.Application.Guests.Repositories;

namespace Hotel.Persistence.Guests;

public class GuestReadRepository(PersistenceDbContext dbContext) : IGuestReadRepository
{
    public async Task<IReadOnlyCollection<GuestListDto>> GetAll(CancellationToken cancellationToken)
    {
        return await dbContext.Guests
            .AsNoTracking()
            .OrderBy(c => c.LastName)
            .ThenBy(c => c.FirstName)
            .Select(c => new GuestListDto(
                c.Id,
                c.FirstName + " " + c.LastName,
                c.Phone,
                c.Email,
                c.DocumentNumber))
            .ToListAsync(cancellationToken);
    }

    public async Task<GuestDto> GetById(Guid id, CancellationToken cancellationToken)
    {
        var guest = await dbContext.Guests
            .AsNoTracking()
            .Where(c => c.Id == id)
            .Select(c => new GuestDto(
                c.Id,
                c.FirstName,
                c.LastName,
                c.Phone,
                c.Email,
                c.DocumentNumber))
            .FirstOrDefaultAsync(cancellationToken);

        if (guest is null)
        {
            throw new NotFoundException($"Guest with id {id} doesn't exist");
        }

        return guest;
    }

    public async Task<IReadOnlyCollection<GuestListDto>> Search(
        string? name,
        string? phone,
        string? email,
        string? documentNumber,
        CancellationToken cancellationToken)
    {
        var query = dbContext.Guests.AsNoTracking().AsQueryable();

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

        return await query
            .OrderBy(c => c.LastName)
            .ThenBy(c => c.FirstName)
            .Select(c => new GuestListDto(
                c.Id,
                c.FirstName + " " + c.LastName,
                c.Phone,
                c.Email,
                c.DocumentNumber))
            .ToListAsync(cancellationToken);
    }
}