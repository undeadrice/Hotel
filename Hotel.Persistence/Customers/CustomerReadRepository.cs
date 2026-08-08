using Hotel.Application.Customers.Services;
using Hotel.Application.Customers.TransferObjects;
using Hotel.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Persistence.Customers;

public class CustomerReadRepository(PersistenceDbContext dbContext) : ICustomerReadRepository
{
    public async Task<IReadOnlyCollection<CustomerListDto>> GetAll(CancellationToken cancellationToken)
    {
        return await dbContext.Customers
            .AsNoTracking()
            .OrderBy(c => c.LastName)
            .ThenBy(c => c.FirstName)
            .Select(c => new CustomerListDto(
                c.Id,
                c.FirstName + " " + c.LastName,
                c.Phone,
                c.Email,
                c.DocumentNumber))
            .ToListAsync(cancellationToken);
    }

    public async Task<CustomerDto> GetById(Guid id, CancellationToken cancellationToken)
    {
        var customer = await dbContext.Customers
            .AsNoTracking()
            .Where(c => c.Id == id)
            .Select(c => new CustomerDto(
                c.Id,
                c.FirstName,
                c.LastName,
                c.Phone,
                c.Email,
                c.DocumentNumber))
            .FirstOrDefaultAsync(cancellationToken);

        if (customer is null)
        {
            throw new NotFoundException($"Customer with id {id} doesn't exist");
        }

        return customer;
    }

    public async Task<IReadOnlyCollection<CustomerListDto>> Search(
        string? name,
        string? phone,
        string? email,
        string? documentNumber,
        CancellationToken cancellationToken)
    {
        var query = dbContext.Customers.AsNoTracking().AsQueryable();

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
            .Select(c => new CustomerListDto(
                c.Id,
                c.FirstName + " " + c.LastName,
                c.Phone,
                c.Email,
                c.DocumentNumber))
            .ToListAsync(cancellationToken);
    }
}