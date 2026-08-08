using MediatR;

namespace Hotel.Application.RatePlans.Commands;

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