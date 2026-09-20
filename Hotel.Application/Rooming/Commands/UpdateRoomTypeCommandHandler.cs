using Hotel.Domain.Rooming.Services;
using MediatR;
using Hotel.Application.Pipeline;
using Hotel.Application.Users.Enums;

namespace Hotel.Application.Rooming.Commands;

[CheckPermission(Permission.RoomTypeEdit)]
public record UpdateRoomTypeCommand(
    Guid Id,
    string Name,
    string? Description)
    : ICommand;

internal class UpdateRoomTypeCommandHandler(IRoomTypeUpdateService roomTypeUpdateService)
    : IRequestHandler<UpdateRoomTypeCommand>
{
    public async Task Handle(UpdateRoomTypeCommand request, CancellationToken cancellationToken)
    {
        await roomTypeUpdateService.UpdateRoomType(request.Id, request.Name, request.Description, cancellationToken);
    }
}
