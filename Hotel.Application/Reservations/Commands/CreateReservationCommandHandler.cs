using Hotel.Domain.FiscalAccounting.Entities;
using Hotel.Domain.FiscalAccounting.Services;
using Hotel.Domain.Reservations.Entities;
using Hotel.Domain.Reservations.Services;
using MediatR;

namespace Hotel.Application.Reservations.Commands;

public class CreateReservationCommandHandler(
    IReservationRepository reservationRepository,
    IFiscalAccountRepository fiscalAccountRepository)
    : IRequestHandler<CreateReservationCommand, Guid>
{
    public async Task<Guid> Handle(CreateReservationCommand request, CancellationToken cancellationToken)
    {
        var reservation = Reservation.Create(
            request.CreatorId,
            request.RoomId,
            request.RatePlanId,
            request.StartDate,
            request.EndDate,
            request.ArrivalTime,
            request.GuestIds);

        await reservationRepository.Add(reservation, cancellationToken);

        var fiscalAccount = FiscalAccount.Create(reservation.Id, request.CreatorId);

        await fiscalAccountRepository.Add(fiscalAccount, cancellationToken);

        return reservation.Id;
    }
}
