using Hotel.Application.Pipeline;
using Hotel.Application.Rooming.TransferObjects;
using Hotel.Application.Users.Enums;
using MediatR;

namespace Hotel.Application.Rooming.Queries;

[CheckPermission(Permission.RoomView)]
public record GetRoomByIdQuery(Guid Id) : IRequest<RoomDto>;
