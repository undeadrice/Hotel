using Hotel.Domain.Rooming.Entities;
using Hotel.Domain.Rooming.Services;
using MediatR;

namespace Hotel.Application.Rooming.Commands;

public class CreateRoomTypeCommandHandler(IRoomTypeRepository roomTypeRepository)
    : IRequestHandler<CreateRoomTypeCommand, Guid>
{
    public async Task<Guid> Handle(CreateRoomTypeCommand request, CancellationToken cancellationToken)
    {
        var roomType = RoomType.Create(request.Name, request.BaseRate, request.Description);

        await roomTypeRepository.Add(roomType);

        return roomType.Id;
    }
}