using Hotel.Domain.RatePlans.Entities;
using Hotel.Domain.RatePlans.Services;
using MediatR;

namespace Hotel.Application.RatePlans.Commands;

public class CreateRatePlanCommandHandler(IRatePlanRepository ratePlanRepository)
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