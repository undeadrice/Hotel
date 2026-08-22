using Hotel.Domain.Reservations.Services;
using MediatR;
using Hotel.Domain.FiscalAccounting.Repositories;

namespace Hotel.Application.FiscalAccounting.Commands;

public class CheckOutFiscalAccountCommandHandler(
    IFiscalAccountRepository fiscalAccountRepository,
    IReservationCheckOutService reservationCheckOutService)
    : IRequestHandler<CheckOutFiscalAccountCommand>
{
    public async Task Handle(CheckOutFiscalAccountCommand request, CancellationToken cancellationToken)
    {
        var account = await fiscalAccountRepository.GetForCheckOut(request.AccountId, cancellationToken);

        account.CheckOut();

        await reservationCheckOutService.CheckOut(account.OriginatorId, cancellationToken);
    }
}
