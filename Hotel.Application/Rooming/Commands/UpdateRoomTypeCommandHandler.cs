using Hotel.Domain.Rooming.Services;
using MediatR;

namespace Hotel.Application.Rooming.Commands;

public class UpdateRoomTypeCommandHandler(IRoomTypeRepository roomTypeRepository)
    : IRequestHandler<UpdateRoomTypeCommand>
{
    public async Task Handle(UpdateRoomTypeCommand request, CancellationToken cancellationToken)
    {
        var roomType = await roomTypeRepository.GetById(request.Id);
        roomType.Update(request.Name, request.BaseRate, request.Description);

        await roomTypeRepository.Update(roomType);
    }
}