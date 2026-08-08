using Hotel.Domain.Transactions.Services;
using MediatR;

namespace Hotel.Application.Transactions.Commands;

public class UpdateTransactionGroupCommandHandler(ITransactionGroupUpdateService transactionGroupUpdateService)
    : IRequestHandler<UpdateTransactionGroupCommand>
{
    public async Task Handle(UpdateTransactionGroupCommand request, CancellationToken cancellationToken)
    {
        await transactionGroupUpdateService.UpdateTransactionGroup(
            request.Id,
            request.Code,
            request.Name,
            request.Type,
            cancellationToken);
    }
}
