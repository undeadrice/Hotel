using Hotel.Application.Rooming.TransferObjects;
using MediatR;
using Hotel.Application.Rooming.Repositories;
using Hotel.Application.Pipeline;
using Hotel.Application.Users.Enums;

namespace Hotel.Application.Rooming.Queries;

[CheckPermission(Permission.RoomTypeView)]
public record GetRoomTypeByIdQuery(Guid Id) : IRequest<RoomTypeDto>;

internal class GetRoomTypeByIdQueryHandler(IRoomTypeReadRepository roomTypeReadRepository)
    : IRequestHandler<GetRoomTypeByIdQuery, RoomTypeDto>
{
    public async Task<RoomTypeDto> Handle(GetRoomTypeByIdQuery request, CancellationToken cancellationToken)
    {
        return await roomTypeReadRepository.GetById(request.Id, cancellationToken);
    }
}
