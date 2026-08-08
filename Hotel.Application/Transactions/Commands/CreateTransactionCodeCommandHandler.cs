using Hotel.Domain.Transactions.Services;
using MediatR;

namespace Hotel.Application.Transactions.Commands;

public class CreateTransactionCodeCommandHandler(ITransactionCodeCreationService transactionCodeCreationService)
    : IRequestHandler<CreateTransactionCodeCommand, Guid>
{
    public async Task<Guid> Handle(CreateTransactionCodeCommand request, CancellationToken cancellationToken)
    {
        var transactionCode = await transactionCodeCreationService.CreateTransactionCode(
            request.TransactionGroupId,
            request.Code,
            request.Name,
            request.Description,
            request.DefaultAmount,
            cancellationToken);

        return transactionCode.Id;
    }
}
