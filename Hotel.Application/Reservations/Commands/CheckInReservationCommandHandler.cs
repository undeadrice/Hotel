using MediatR;
using Hotel.Domain.Reservations.Repositories;
using Hotel.Application.Pipeline;
using Hotel.Application.Users.Enums;

namespace Hotel.Application.Reservations.Commands;

[CheckPermission(Permission.ReservationEdit)]
public record CheckInReservationCommand(Guid ReservationId) : ICommand;

public class CheckInReservationCommandHandler(IReservationRepository reservationRepository)
    : IRequestHandler<CheckInReservationCommand>
{
    public async Task Handle(CheckInReservationCommand request, CancellationToken cancellationToken)
    {
        var reservation = await reservationRepository.GetById(request.ReservationId, cancellationToken);

        reservation.CheckIn();
    }
}
