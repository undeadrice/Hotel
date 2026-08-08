using Hotel.Domain.Transactions.Services;
using MediatR;

namespace Hotel.Application.Transactions.Commands;

public class UpdateTransactionCodeCommandHandler(ITransactionCodeUpdateService transactionCodeUpdateService)
    : IRequestHandler<UpdateTransactionCodeCommand>
{
    public async Task Handle(UpdateTransactionCodeCommand request, CancellationToken cancellationToken)
    {
        await transactionCodeUpdateService.UpdateTransactionCode(
            request.Id,
            request.TransactionGroupId,
            request.Code,
            request.Name,
            request.Description,
            request.DefaultAmount,
            cancellationToken);
    }
}
