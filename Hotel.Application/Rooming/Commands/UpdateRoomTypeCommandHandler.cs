using MediatR;
using Hotel.Domain.Rooming.Repositories;
using Hotel.Application.Pipeline;
using Hotel.Application.Users.Enums;

namespace Hotel.Application.Rooming.Commands;

[CheckPermission(Permission.RoomTypeEdit)]
public record UpdateRoomTypeCommand(
    Guid Id,
    string Name,
    string? Description)
    : ICommand;

public class UpdateRoomTypeCommandHandler(IRoomTypeRepository roomTypeRepository)
    : IRequestHandler<UpdateRoomTypeCommand>
{
    public async Task Handle(UpdateRoomTypeCommand request, CancellationToken cancellationToken)
    {
        var roomType = await roomTypeRepository.GetById(request.Id);
        roomType.Update(request.Name, request.Description);
    }
}
