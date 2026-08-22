using Hotel.Domain.RatePlans.Entities;
using MediatR;
using Hotel.Domain.RatePlans.Repositories;

namespace Hotel.Application.RatePlans.Commands;

public class UpdateRatePlanCommandHandler(IRatePlanRepository ratePlanRepository)
    : IRequestHandler<UpdateRatePlanCommand>
{
    public async Task Handle(UpdateRatePlanCommand request, CancellationToken cancellationToken)
    {
        var ratePlan = await ratePlanRepository.GetById(request.Id, cancellationToken);

        var rooms = request.Rooms.Select(r => new RoomTypePriceDefinition(r.RoomTypeId, r.Price));

        ratePlan.Update(
            request.Name,
            request.TransactionCodeId,
            request.StartDate,
            request.EndDate,
            rooms);
    }
}