using Hotel.Domain.RatePlans.Services;
using MediatR;

namespace Hotel.Application.RatePlans.Commands;

public class UpdateRatePlanCommandHandler(IRatePlanRepository ratePlanRepository)
    : IRequestHandler<UpdateRatePlanCommand>
{
    public async Task Handle(UpdateRatePlanCommand request, CancellationToken cancellationToken)
    {
        var ratePlan = await ratePlanRepository.GetById(request.Id, cancellationToken);

        var rooms = request.Rooms.Select(r => (r.RoomTypeId, r.Price));

        ratePlan.Update(
            request.Name,
            request.TransactionCodeId,
            request.StartDate,
            request.EndDate,
            rooms);

        await ratePlanRepository.Update(ratePlan, cancellationToken);
    }
}