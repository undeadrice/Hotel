using Hotel.Application.Rooming.Services;
using Hotel.Application.Rooming.TransferObjects;
using Hotel.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Persistence.Rooming;

public class RoomReadRepository(PersistenceDbContext dbContext) : IRoomReadRepository
{
    public async Task<IReadOnlyCollection<RoomListDto>> GetAll(CancellationToken cancellationToken)
    {
        return await dbContext.Rooms
            .AsNoTracking()
            .OrderBy(r => r.RoomNumber)
            .Select(r => new RoomListDto(r.Id, r.RoomNumber))
            .ToListAsync(cancellationToken);
    }

    public async Task<RoomDto> GetById(Guid id, CancellationToken cancellationToken)
    {
        var room = await dbContext.Rooms
            .AsNoTracking()
            .Include(r => r.RoomType)
            .Where(r => r.Id == id)
            .Select(r => new RoomDto(r.Id, r.RoomNumber, r.RoomTypeId, r.RoomType.Name, r.Status, r.IsActive))
            .FirstOrDefaultAsync(cancellationToken);

        if (room is null)
        {
            throw new NotFoundException($"Room with id {id} doesn't exist");
        }

        return room;
    }
}
