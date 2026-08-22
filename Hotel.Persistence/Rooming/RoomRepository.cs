using Hotel.Domain.Rooming.Entities;
using Hotel.Domain.Rooming.Services;
using Hotel.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Persistence.Rooming;

public class RoomRepository(PersistenceDbContext persistenceDbContext) : IRoomRepository
{
    public async Task Add(Room room, CancellationToken token)
    {
        await persistenceDbContext.Rooms.AddAsync(room);
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

    public async Task<bool> ExistsByRoomNumber(string roomNumber, CancellationToken token)
    {
        return await persistenceDbContext.Rooms
            .AnyAsync(x => x.RoomNumber == roomNumber, token);
    }

    public async Task<bool> ExistsByRoomNumberExcluding(Guid roomId, string roomNumber, CancellationToken token)
    {
        return await persistenceDbContext.Rooms
            .AnyAsync(x => x.RoomNumber == roomNumber && x.Id != roomId, token);
    }
}
