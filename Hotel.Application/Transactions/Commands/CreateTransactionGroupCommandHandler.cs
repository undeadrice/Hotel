using Hotel.Domain.Transactions.Services;
using MediatR;

namespace Hotel.Application.Transactions.Commands;

public class CreateTransactionGroupCommandHandler(ITransactionGroupCreationService transactionGroupCreationService)
    : IRequestHandler<CreateTransactionGroupCommand, Guid>
{
    public async Task<Guid> Handle(CreateTransactionGroupCommand request, CancellationToken cancellationToken)
    {
        var transactionGroup = await transactionGroupCreationService.CreateTransactionGroup(
            request.Code,
            request.Name,
            request.Description,
            request.Type,
            cancellationToken);

        return transactionGroup.Id;
    }
}
