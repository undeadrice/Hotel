using MediatR;
using Hotel.Domain.Transactions.Repositories;
using Hotel.Application.Pipeline;
using Hotel.Application.Users.Enums;

namespace Hotel.Application.Transactions.Commands;

[CheckPermission(Permission.TransactionGroupEdit)]
public record ChangeTransactionGroupStatusCommand(
    Guid Id,
    bool IsActive)
    : ICommand;

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
    }
}
