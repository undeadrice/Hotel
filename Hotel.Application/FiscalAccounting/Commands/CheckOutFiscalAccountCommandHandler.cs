using Hotel.Domain.FiscalAccounting.Services;
using Hotel.Domain.Reservations.Services;
using MediatR;

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
