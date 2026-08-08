using Hotel.Domain.Customers;
using Hotel.Domain.Customers.Services;
using Hotel.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Hotel.Persistence.Customers;

public class CustomerRepository(PersistenceDbContext persistenceDbContext) : ICustomerRepository
{
    public async Task Add(Customer customer, CancellationToken token)
    {
        await persistenceDbContext.Customers.AddAsync(customer, token);
    }

    public async Task Update(Customer customer, CancellationToken token)
    {
        persistenceDbContext.Customers.Update(customer);
    }

    public async Task<Customer> GetById(Guid id, CancellationToken token)
    {
        var result = await persistenceDbContext.Customers.FirstOrDefaultAsync(x => x.Id == id, cancellationToken: token);

        if (result == null)
        {
            throw new NotFoundException($"Customer with id {id} doesn't exist");
        }

        return result;
    }

    public async Task<Customer?> FindById(Guid id, CancellationToken token)
    {
        return await persistenceDbContext.Customers.FirstOrDefaultAsync(x => x.Id == id, cancellationToken: token);
    }

    public async Task<IReadOnlyCollection<Customer>> GetAll(CancellationToken token, Expression<Func<Customer, bool>>? filter = null)
    {
        if (filter == null)
        {
            return await persistenceDbContext.Customers.ToListAsync(cancellationToken: token);
        }

        return await persistenceDbContext.Customers.Where(filter).ToListAsync(cancellationToken: token);
    }

    public async Task<IReadOnlyCollection<Customer>> Search(
        string? name,
        string? phone,
        string? email,
        string? documentNumber,
        CancellationToken token = default)
    {
        var query = persistenceDbContext.Customers.AsQueryable();

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