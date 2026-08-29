using Hotel.Application.Rooming.TransferObjects;
using MediatR;
using Hotel.Application.Rooming.Repositories;
using Hotel.Application.Pipeline;
using Hotel.Application.Users.Enums;

namespace Hotel.Application.Rooming.Queries;

[CheckPermission(Permission.RoomView)]
public record GetRoomByIdQuery(Guid Id) : IRequest<RoomDto>;

internal class GetRoomByIdQueryHandler(IRoomReadRepository roomReadRepository)
    : IRequestHandler<GetRoomByIdQuery, RoomDto>
{
    public async Task<RoomDto> Handle(GetRoomByIdQuery request, CancellationToken cancellationToken)
    {
        return await roomReadRepository.GetById(request.Id, cancellationToken);
    }
}
