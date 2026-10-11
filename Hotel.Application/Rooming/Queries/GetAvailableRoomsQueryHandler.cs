using Hotel.Application.Rooming.TransferObjects;
using MediatR;
using Hotel.Application.Rooming.Repositories;
using Hotel.Application.Pipeline;
using Hotel.Application.Users.Enums;

namespace Hotel.Application.Rooming.Queries;

[CheckPermission(Permission.RoomView)]
public record GetAvailableRoomsQuery(DateOnly StartDate, DateOnly EndDate)
    : IRequest<IReadOnlyCollection<RoomListDto>>;

internal class GetAvailableRoomsQueryHandler(IRoomReadRepository roomReadRepository)
    : IRequestHandler<GetAvailableRoomsQuery, IReadOnlyCollection<RoomListDto>>
{
    public async Task<IReadOnlyCollection<RoomListDto>> Handle(
        GetAvailableRoomsQuery request,
        CancellationToken cancellationToken)
    {
        return await roomReadRepository.GetAvailable(
            request.StartDate,
            request.EndDate,
            cancellationToken);
    }
}
