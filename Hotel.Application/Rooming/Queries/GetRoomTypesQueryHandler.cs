using Hotel.Application.Rooming.TransferObjects;
using MediatR;
using Hotel.Application.Rooming.Repositories;
using Hotel.Application.Pipeline;
using Hotel.Application.Users.Enums;

namespace Hotel.Application.Rooming.Queries;

[CheckPermission(Permission.RoomTypeView)]
public record GetRoomTypesQuery() : IRequest<IReadOnlyCollection<RoomTypeListDto>>;

internal class GetRoomTypesQueryHandler(IRoomTypeReadRepository roomTypeReadRepository)
    : IRequestHandler<GetRoomTypesQuery, IReadOnlyCollection<RoomTypeListDto>>
{
    public async Task<IReadOnlyCollection<RoomTypeListDto>> Handle(GetRoomTypesQuery request, CancellationToken cancellationToken)
    {
        return await roomTypeReadRepository.GetAll(cancellationToken);
    }
}
