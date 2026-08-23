using MediatR;
using Hotel.Domain.FiscalAccounting.Repositories;
using Hotel.Domain.Reservations.Repositories;

namespace Hotel.Application.FiscalAccounting.Commands;

public class CheckOutFiscalAccountCommandHandler(
    IFiscalAccountRepository fiscalAccountRepository,
    IReservationRepository reservationRepository)
    : IRequestHandler<CheckOutFiscalAccountCommand>
{
    public async Task Handle(CheckOutFiscalAccountCommand request, CancellationToken cancellationToken)
    {
        var account = await fiscalAccountRepository.GetForCheckOut(request.AccountId, cancellationToken);

        account.CheckOut();

        var reservation = await reservationRepository.GetById(account.OriginatorId, cancellationToken);

        reservation.CheckOut();
    }
}
