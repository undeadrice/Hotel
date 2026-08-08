using Hotel.Domain.Rooming.Entities;
using Hotel.Domain.Rooming.Services;
using MediatR;

namespace Hotel.Application.Rooming.Commands;

public class CreateRoomCommandHandler(IRoomRepository roomRepository)
    : IRequestHandler<CreateRoomCommand, Guid>
{
    public async Task<Guid> Handle(CreateRoomCommand request, CancellationToken cancellationToken)
    {
        var room = Room.Create(request.RoomNumber, request.RoomTypeId);

        await roomRepository.Add(room, cancellationToken);

        return room.Id;
    }
}