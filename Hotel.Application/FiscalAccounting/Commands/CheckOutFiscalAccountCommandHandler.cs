using Hotel.Domain.FiscalAccounting.Services;
using MediatR;

namespace Hotel.Application.FiscalAccounting.Commands;

public class CheckOutFiscalAccountCommandHandler(IFiscalAccountRepository fiscalAccountRepository)
    : IRequestHandler<CheckOutFiscalAccountCommand>
{
    public async Task Handle(CheckOutFiscalAccountCommand request, CancellationToken cancellationToken)
    {
        var account = await fiscalAccountRepository.GetForCheckOut(request.AccountId, cancellationToken);

        account.CheckOut();
    }
}