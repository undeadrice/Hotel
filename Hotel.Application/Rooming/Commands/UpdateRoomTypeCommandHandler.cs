using MediatR;
using Hotel.Domain.Rooming.Repositories;

namespace Hotel.Application.Rooming.Commands;

public class UpdateRoomTypeCommandHandler(IRoomTypeRepository roomTypeRepository)
    : IRequestHandler<UpdateRoomTypeCommand>
{
    public async Task Handle(UpdateRoomTypeCommand request, CancellationToken cancellationToken)
    {
        var roomType = await roomTypeRepository.GetById(request.Id);
        roomType.Update(request.Name, request.Description);
    }
}