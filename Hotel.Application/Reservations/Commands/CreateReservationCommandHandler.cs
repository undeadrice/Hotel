using Hotel.Domain.Reservations.Services;
using MediatR;

namespace Hotel.Application.Reservations.Commands;

public class CreateReservationCommandHandler(IRoomReservationService roomReservationService)
    : IRequestHandler<CreateReservationCommand, Guid>
{
    public async Task<Guid> Handle(CreateReservationCommand request, CancellationToken cancellationToken)
    {
        var reservation = await roomReservationService.CreateReservation(
            request.CreatorId,
            request.RoomId,
            request.RatePlanId,
            request.StartDate,
            request.EndDate,
            request.ArrivalTime,
            request.GuestIds,
            cancellationToken);

        return reservation.Id;
    }
}