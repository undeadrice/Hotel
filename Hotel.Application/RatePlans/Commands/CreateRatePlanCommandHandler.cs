using Hotel.Domain.RatePlans.Entities;
using MediatR;
using Hotel.Domain.RatePlans.Repositories;
using Hotel.Application.Pipeline;
using Hotel.Application.Users.Enums;

namespace Hotel.Application.RatePlans.Commands;

[CheckPermission(Permission.RatePlanCreate)]
public record CreateRatePlanCommand(
    string Name,
    Guid TransactionCodeId,
    DateOnly StartDate,
    DateOnly EndDate,
    List<CreateRatePlanRoomCommand> Rooms) : ICommand<Guid>;

public record CreateRatePlanRoomCommand(
    Guid RoomTypeId,
    decimal Price);

internal class CreateRatePlanCommandHandler(IRatePlanRepository ratePlanRepository)
    : IRequestHandler<CreateRatePlanCommand, Guid>
{
    public async Task<Guid> Handle(CreateRatePlanCommand request, CancellationToken cancellationToken)
    {
        var rooms = request.Rooms.Select(r => new RoomTypePriceDefinition(r.RoomTypeId, r.Price));

        var ratePlan = RatePlan.Create(
            request.Name,
            request.TransactionCodeId,
            request.StartDate,
            request.EndDate,
            rooms);

        await ratePlanRepository.Add(ratePlan, cancellationToken);

        return ratePlan.Id;
    }
}
