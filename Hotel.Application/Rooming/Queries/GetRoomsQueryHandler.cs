using Hotel.Application.Rooming.TransferObjects;
using MediatR;
using Hotel.Application.Rooming.Repositories;
using Hotel.Application.Pipeline;
using Hotel.Application.Users.Enums;

namespace Hotel.Application.Rooming.Queries;

[CheckPermission(Permission.RoomView)]
public record GetRoomsQuery() : IRequest<IReadOnlyCollection<RoomListDto>>;

internal class GetRoomsQueryHandler(IRoomReadRepository roomReadRepository)
    : IRequestHandler<GetRoomsQuery, IReadOnlyCollection<RoomListDto>>
{
    public async Task<IReadOnlyCollection<RoomListDto>> Handle(GetRoomsQuery request, CancellationToken cancellationToken)
    {
        return await roomReadRepository.GetAll(cancellationToken);
    }
}
