using Hotel.Domain.RatePlans.Entities;
using MediatR;
using Hotel.Domain.RatePlans.Repositories;
using Hotel.Application.Pipeline;
using Hotel.Application.Users.Enums;
using Hotel.Application.Configurations.Services;

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

internal class UpdateRatePlanCommandHandler(
    IRatePlanRepository ratePlanRepository,
    IBusinessDateProvider businessDateProvider)
    : IRequestHandler<UpdateRatePlanCommand>
{
    public async Task Handle(UpdateRatePlanCommand request, CancellationToken cancellationToken)
    {
        var ratePlan = await ratePlanRepository.GetById(request.Id, cancellationToken);

        var businessDate = await businessDateProvider.GetCurrentBusinessDate(cancellationToken);

        var rooms = request.Rooms.Select(r => new RoomTypePriceDefinition(r.RoomTypeId, r.Price));

        ratePlan.Update(
            request.Name,
            request.TransactionCodeId,
            request.StartDate,
            request.EndDate,
            businessDate,
            rooms);
    }
}
