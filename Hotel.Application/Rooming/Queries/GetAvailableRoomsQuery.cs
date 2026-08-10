using Hotel.Application.Rooming.TransferObjects;
using MediatR;

namespace Hotel.Application.Rooming.Queries;

public record GetAvailableRoomsQuery(DateOnly StartDate, DateOnly EndDate)
    : IRequest<IReadOnlyCollection<RoomListDto>>;
