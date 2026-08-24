using Hotel.Domain.Rooming.Entities;
using Hotel.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;
using Hotel.Domain.Rooming.Repositories;

namespace Hotel.Persistence.Rooming;

public class RoomTypeRepository(PersistenceDbContext persistenceDbContext) : IRoomTypeRepository
{
    public async Task Add(RoomType roomType, CancellationToken token)
    {
        await persistenceDbContext.RoomTypes.AddAsync(roomType);
    }

    public async Task<RoomType> GetById(Guid id, CancellationToken token)
    {
        var result = await persistenceDbContext.RoomTypes.FirstOrDefaultAsync(x => x.Id == id);

        if (result == null)
        {
            throw new NotFoundException($"RoomType with id {id} doesn't exist");
        }

        return result;
    }

    public async Task<RoomType?> FindById(Guid id, CancellationToken token)
    {
        return await persistenceDbContext.RoomTypes.FirstOrDefaultAsync(x => x.Id == id);
    }
}
