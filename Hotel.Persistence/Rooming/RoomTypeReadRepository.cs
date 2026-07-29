using Hotel.Application.Rooming.Services;
using Hotel.Application.Rooming.TransferObjects;
using Hotel.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Persistence.Rooming;

public class RoomTypeReadRepository(PersistenceDbContext dbContext) : IRoomTypeReadRepository
{
    public async Task<IReadOnlyCollection<RoomTypeListDto>> GetAll(CancellationToken cancellationToken)
    {
        return await dbContext.RoomTypes
            .AsNoTracking()
            .OrderBy(rt => rt.Name)
            .Select(rt => new RoomTypeListDto(rt.Id, rt.Name))
            .ToListAsync(cancellationToken);
    }

    public async Task<RoomTypeDto> GetById(Guid id, CancellationToken cancellationToken)
    {
        var roomType = await dbContext.RoomTypes
            .AsNoTracking()
            .Where(rt => rt.Id == id)
            .Select(rt => new RoomTypeDto(rt.Id, rt.Name, rt.BaseRate, rt.Description))
            .FirstOrDefaultAsync(cancellationToken);

        if (roomType is null)
        {
            throw new NotFoundException($"Room type with id {id} doesn't exist");
        }

        return roomType;
    }
}
