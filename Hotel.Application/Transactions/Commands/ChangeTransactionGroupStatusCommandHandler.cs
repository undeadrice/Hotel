using Hotel.Domain.Transactions.Services;
using MediatR;

namespace Hotel.Application.Transactions.Commands;

public class ChangeTransactionGroupStatusCommandHandler(ITransactionGroupRepository transactionGroupRepository)
    : IRequestHandler<ChangeTransactionGroupStatusCommand>
{
    public async Task Handle(ChangeTransactionGroupStatusCommand request, CancellationToken cancellationToken)
    {
        var transactionGroup = await transactionGroupRepository.GetById(request.Id, cancellationToken);

        if (request.IsActive)
        {
            transactionGroup.Activate();
        }
        else
        {
            transactionGroup.Deactivate();
        }

        await transactionGroupRepository.Update(transactionGroup, cancellationToken);
    }
}
