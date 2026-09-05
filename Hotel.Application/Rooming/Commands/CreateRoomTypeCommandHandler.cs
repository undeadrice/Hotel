using Hotel.Domain.Rooming.Entities;
using Hotel.Domain.Rooming.Services;
using MediatR;
using Hotel.Domain.Rooming.Repositories;
using Hotel.Application.Pipeline;
using Hotel.Application.Users.Enums;

namespace Hotel.Application.Rooming.Commands;

[CheckPermission(Permission.RoomTypeCreate)]
public record CreateRoomTypeCommand(
    string Name,
    string? Description)
    : ICommand<Guid>;

public class CreateRoomTypeCommandHandler(IRoomTypeRepository roomTypeRepository)
    : IRequestHandler<CreateRoomTypeCommand, Guid>
{
    public async Task<Guid> Handle(CreateRoomTypeCommand request, CancellationToken cancellationToken)
    {
        var roomType = RoomType.Create(request.Name, request.Description);

        await roomTypeRepository.Add(roomType);

        return roomType.Id;
    }
}
