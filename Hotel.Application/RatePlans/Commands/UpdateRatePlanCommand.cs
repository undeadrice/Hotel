using Hotel.Application.Pipeline;
using Hotel.Application.Users.Enums;
using MediatR;

namespace Hotel.Application.RatePlans.Commands;

[CheckPermission(Permission.RatePlanEdit)]
public record UpdateRatePlanCommand(
    Guid Id,
    string Name,
    Guid TransactionCodeId,
    DateOnly StartDate,
    DateOnly EndDate,
    List<UpdateRatePlanRoomCommand> Rooms) : IRequest;

public record UpdateRatePlanRoomCommand(
    Guid RoomTypeId,
    decimal Price);