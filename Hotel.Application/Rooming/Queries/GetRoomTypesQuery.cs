using Hotel.Application.Pipeline;
using Hotel.Application.Rooming.TransferObjects;
using Hotel.Application.Users.Enums;
using MediatR;

namespace Hotel.Application.Rooming.Queries;

[CheckPermission(Permission.RoomTypeView)]
public record GetRoomTypesQuery() : IRequest<IReadOnlyCollection<RoomTypeListDto>>;
