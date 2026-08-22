using Hotel.Domain.Reservations.Services;
using MediatR;
using Hotel.Domain.Reservations.Repositories;

namespace Hotel.Application.Reservations.Commands;

public class CheckInReservationCommandHandler(IReservationRepository reservationRepository)
    : IRequestHandler<CheckInReservationCommand>
{
    public async Task Handle(CheckInReservationCommand request, CancellationToken cancellationToken)
    {
        var reservation = await reservationRepository.GetById(request.ReservationId, cancellationToken);

        reservation.CheckIn();
    }
}