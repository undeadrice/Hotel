using MediatR;

namespace Hotel.Application.RatePlans.Commands;

public record CreateRatePlanCommand(
    string Name,
    Guid TransactionCodeId,
    DateOnly StartDate,
    DateOnly EndDate,
    List<CreateRatePlanRoomCommand> Rooms) : IRequest<Guid>;

public record CreateRatePlanRoomCommand(
    Guid RoomTypeId,
    decimal Price);