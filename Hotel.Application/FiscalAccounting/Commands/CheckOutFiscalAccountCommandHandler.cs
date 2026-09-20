using MediatR;
using Hotel.Domain.FiscalAccounting.Repositories;
using Hotel.Domain.Reservations.Repositories;
using Hotel.Application.Pipeline;
using Hotel.Application.Users.Enums;

namespace Hotel.Application.FiscalAccounting.Commands;

[CheckPermission(Permission.FiscalAccountEdit)]
public record CheckOutFiscalAccountCommand(Guid AccountId) : ICommand;

internal class CheckOutFiscalAccountCommandHandler(
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
