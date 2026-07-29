using Hotel.Application.Rooming.TransferObjects;
using MediatR;

namespace Hotel.Application.Rooming.Queries;

public record GetRoomTypesQuery() : IRequest<IReadOnlyCollection<RoomTypeListDto>>;
