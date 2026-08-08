using Hotel.Domain.Rooming.Entities;
using Hotel.Domain.Rooming.Services;
using Hotel.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Hotel.Persistence.Rooming;

public class RoomRepository(PersistenceDbContext persistenceDbContext) : IRoomRepository
{
    public async Task Add(Room room, CancellationToken token)
    {
        await persistenceDbContext.Rooms.AddAsync(room);
    }

    public async Task Update(Room room, CancellationToken token)
    {
        persistenceDbContext.Rooms.Update(room);
    }

    public async Task<Room> GetById(Guid id, CancellationToken token)
    {
        var result = await persistenceDbContext.Rooms
            .FirstOrDefaultAsync(x => x.Id == id);

        if (result == null)
        {
            throw new NotFoundException($"Room with id {id} doesn't exist");
        }

        return result;
    }

    public async Task<Room?> FindById(Guid id, CancellationToken token)
    {
        return await persistenceDbContext.Rooms
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IReadOnlyCollection<Room>> GetAll(CancellationToken token, Expression<Func<Room, bool>>? filter = null)
    {
        var query = persistenceDbContext.Rooms
            .AsQueryable();

        if (filter != null)
        {
            query = query.Where(filter);
        }

        return await query.ToListAsync();
    }
}