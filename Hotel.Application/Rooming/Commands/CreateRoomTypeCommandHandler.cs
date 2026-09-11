using Hotel.Domain.Rooming.Services;
using MediatR;
using Hotel.Application.Pipeline;
using Hotel.Application.Users.Enums;

namespace Hotel.Application.Rooming.Commands;

[CheckPermission(Permission.RoomTypeCreate)]
public record CreateRoomTypeCommand(
    string Name,
    string? Description)
    : ICommand<Guid>;

internal class CreateRoomTypeCommandHandler(IRoomTypeCreationService roomTypeCreationService)
    : IRequestHandler<CreateRoomTypeCommand, Guid>
{
    public async Task<Guid> Handle(CreateRoomTypeCommand request, CancellationToken cancellationToken)
    {
        var roomType = await roomTypeCreationService.CreateRoomType(request.Name, request.Description, cancellationToken);

        return roomType.Id;
    }
}
