using Hotel.Application.Rooming.TransferObjects;
using MediatR;

namespace Hotel.Application.Rooming.Queries;

public record GetRoomTypeByIdQuery(Guid Id) : IRequest<RoomTypeDto>;