using Hotel.Domain.Transactions.Services;
using MediatR;
using Hotel.Domain.Transactions.Repositories;

namespace Hotel.Application.Transactions.Commands;

public class ChangeTransactionCodeStatusCommandHandler(ITransactionCodeRepository transactionCodeRepository)
    : IRequestHandler<ChangeTransactionCodeStatusCommand>
{
    public async Task Handle(ChangeTransactionCodeStatusCommand request, CancellationToken cancellationToken)
    {
        var transactionCode = await transactionCodeRepository.GetById(request.Id, cancellationToken);

        if (request.IsActive)
        {
            transactionCode.Activate();
        }
        else
        {
            transactionCode.Deactivate();
        }
    }
}
